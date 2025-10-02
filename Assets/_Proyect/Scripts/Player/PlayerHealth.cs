using UnityEngine;
using Fusion;

public class PlayerHealth : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnHealthChanged))] public int health { get; set; } = 100;

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputPlayer inputPlayer))
        {
            if (inputPlayer.buttons.IsSet(NetworkInputPlayer.MOUSE_BUTTON_1))
            {
                TakeDamage(10);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if(!HasStateAuthority) return;

        health -= damage;
    }

    private void OnHealthChanged()
    {
        Debug.Log($"Has State Auth: [{HasStateAuthority}]. Has Input Auth: [{HasInputAuthority}]. OnHealthChanged: [{health}]");
    }  
}
