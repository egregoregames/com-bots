using System;
using ComBots.Utils.EntryPoints;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : EntryPointMono
{
    public GameObject player;
    public GameObject cameraPlayer;
    public GameObject sceneCam;

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
        var playerGo = Instantiate(player, transform.position, Quaternion.identity);
        var camGo = Instantiate(cameraPlayer, transform.position, Quaternion.identity);

        var sceneCam = Instantiate(this.sceneCam, transform.position, Quaternion.identity);

        var cmCam = camGo.GetComponent<CinemachineCamera>();

        var playerT = playerGo.transform;
        cmCam.Follow = playerT;
        cmCam.LookAt = playerT;

        var thirdPersonController = playerGo.GetComponent<ThirdPersonController>();
        thirdPersonController._controller = playerGo.GetComponent<CharacterController>();
        thirdPersonController._mainCamera = sceneCam;
        thirdPersonController.CinemachineCameraTarget = playerGo;
    }
}

