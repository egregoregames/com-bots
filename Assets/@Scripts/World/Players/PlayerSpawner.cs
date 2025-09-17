using ComBots.Game.Players;
using ComBots.Utils.EntryPoints;
using UnityEngine;

public class PlayerSpawner : EntryPointMono
{
    [SerializeField] private GameObject _playerPrefab;

    public override Dependency Dependency => Dependency.Independent;

    protected override void Init()
    {
        SpawnPlayer();
    }

    public override void Dispose()
    {
        
    }

    [ContextMenu("Spawn Player")]
    public void SpawnPlayer()
    {
        var playerGo = Instantiate(_playerPrefab, transform.position, Quaternion.identity);

        Player player = playerGo.GetComponent<Player>();

        player.Controller._controller = playerGo.GetComponent<CharacterController>();
        player.Controller._mainCamera = player.PlayerCamera.Camera.gameObject;
        player.Controller.CinemachineCameraTarget = playerGo;
    }
}

