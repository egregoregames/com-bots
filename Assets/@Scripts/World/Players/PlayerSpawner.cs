using ComBots.Game.Players;
using ComBots.Utils.EntryPoints;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourR3
{
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _spawnedPlayer;

    protected override void Initialize()
    {
        SpawnPlayer();
    }

    [ContextMenu("Spawn Player")]
    public void SpawnPlayer()
    {
        if (_spawnedPlayer != null)
        {
            Debug.LogWarning("Player already spawned!");
            return;
        }
        _spawnedPlayer = Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        Player player = _spawnedPlayer.GetComponent<Player>();

        player.Controller._controller = _spawnedPlayer.GetComponent<CharacterController>();
        player.Controller._mainCamera = player.PlayerCamera.Camera.gameObject;
        player.Controller.CinemachineCameraTarget = _spawnedPlayer;
    }
}

