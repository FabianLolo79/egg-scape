using UnityEngine;
using Fusion;
using System.Collections.Generic;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int health { get; set; } = 100;

    [SerializeField] private MeshRenderer _renderer;
    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputPlayer inputPlayer))
        {
            if (inputPlayer.buttons.IsSet(NetworkInputPlayer.MOUSE_BUTTON_1))
            {
                RPC_RequestDamage(10);
            }
        }
    }

    public override void Render()
    {
        foreach (var changeVariable in _changeDetector.DetectChanges(this))
        {
            if (changeVariable == nameof(health))
            {
                Debug.Log($"Health changed to {health}");
            }
        }
    }

    //Server Logic
    private void ServerTakeDamage(int damage)
    {
        if(!HasStateAuthority) return;

        health = Mathf.Max(0, health - damage);
    }

    
    // Client (InputAuthority) --> Server
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestDamage(int damage, RpcInfo info = default)
    {
        if (!HasInputAuthority) return;

        ServerTakeDamage(damage);

        RPC_HitDamageFeedback();
    }

    //Server --> Clients (Alls)
    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    private void RPC_HitDamageFeedback(RpcInfo info = default)
    {
        StartCoroutine(DamageFeedBack());
    }

    private IEnumerator DamageFeedBack()
    {
        var initialColor = _renderer.material.color;
        _renderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.3f);
        _renderer.material.color = initialColor;
    }

}
