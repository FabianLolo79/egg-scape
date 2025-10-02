using Fusion;
//using UnityEditor.Networking;
using UnityEngine;

[RequireComponent(typeof(NetworkCharacterController))]
public class PlayerNicoi : NetworkBehaviour
{

    private NetworkCharacterController _characterController;
    [SerializeField] private Renderer _renderer;

    [SerializeField] private GameObject _proyectilePrefab;
    [SerializeField] private Transform _proyectileSpawnPoint;

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
        if (!GetInput(out NetworkInputPlayer inputPlayer)) return;
        
        inputPlayer.moveDirection.Normalize();
        _characterController.Move(inputPlayer.moveDirection * Runner.DeltaTime);

        if (inputPlayer.buttons.IsSet(NetworkInputPlayer.MOUSE_BUTTON_0) && HasStateAuthority)
        {
            var spawnedProyectile = Runner.Spawn(_proyectilePrefab, _proyectileSpawnPoint.position, Quaternion.LookRotation(transform.forward), Object.InputAuthority);
            spawnedProyectile.GetComponent<Proyectile>().InitProyectile();
            Debug.Log("GO proyectile GO");
        }
    }
}
