using Fusion;
//using UnityEditor.Networking;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterController))]
public class PlayerNicoi : NetworkBehaviour
{

    private NetworkCharacterController _characterController;
    [SerializeField] private Renderer _renderer;

    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterController>();
    }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            _renderer.material.color = Color.red;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputPlayer inputPlayer))
        {
            inputPlayer.moveDirection.Normalize();
            _characterController.Move(inputPlayer.moveDirection * Runner.DeltaTime);
        }
        
    }
}
