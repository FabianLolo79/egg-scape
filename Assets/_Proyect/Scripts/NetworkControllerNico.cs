using UnityEngine;
using Random = UnityEngine.Random;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class NetworkControllerNico : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("UI Elements")]
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _joinRoomButton;

    [Header("Network Components")]
    [SerializeField] private NetworkRunner _networkRunner;
    [SerializeField] private NetworkSceneManagerDefault _networkSceneManagerDefault;

    [SerializeField] private NetworkObject _playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _players = new Dictionary<PlayerRef, NetworkObject>();

    private void Start()
    {
        _createRoomButton.onClick.AddListener(CreateRoom);
        _joinRoomButton.onClick.AddListener(JoinRoom);
    }

    private async void CreateRoom()
    {
        var gameArg = new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName = "Room_1",
            SceneManager = _networkSceneManagerDefault,
        };

        var result = await _networkRunner.StartGame(gameArg);

        if (!result.Ok)
        {
            Debug.Log(result.ShutdownReason);
        }
    }

    private async void JoinRoom()
    {
        var gameArg = new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName = "Room_1",
            SceneManager = _networkSceneManagerDefault,
        };

        var result = await _networkRunner.StartGame(gameArg);

        if (!result.Ok)
        {
            Debug.Log(result.ShutdownReason);
        }
    }


    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("OnPlayerJoined");
        _lobbyPanel.SetActive(false);

        if (!_networkRunner.IsServer) return;

        var playerSpawner = _networkRunner.Spawn(_playerPrefab, new Vector3(Random.Range(-3,3), 0, 0), Quaternion.identity, player);
        _players.Add(player, playerSpawner);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {

        if (!_networkRunner.IsServer) return;

        if (_players.Remove(player, out var playerSpawned))
        {
            _networkRunner.Despawn(playerSpawned);
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputPlayer = new NetworkInputPlayer();

        if (Input.GetKey(KeyCode.A))
        {
            inputPlayer.moveDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputPlayer.moveDirection += Vector3.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            inputPlayer.moveDirection += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputPlayer.moveDirection += Vector3.back;
        }

        input.Set(inputPlayer);
    }



    //========================================================\\


    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }


    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

   

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
