using System;
using ComBots.Utils.EntryPoints;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : EntryPointMono
{
    public GameObject player;
    public GameObject camLock;
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
        var camLockGo = Instantiate(camLock, transform.position, Quaternion.identity);

        var sceneCam = Instantiate(this.sceneCam, transform.position, Quaternion.identity);

        camLockGo.GetComponent<CameraLock>().follower = playerGo;
        
        var cmCam = camGo.GetComponent<CinemachineCamera>();
        
        cmCam.Follow = camLockGo.transform;
        cmCam.LookAt = playerGo.transform;
        
        var thirdPersonController = playerGo.GetComponent<ThirdPersonController>();
        thirdPersonController._controller = playerGo.GetComponent<CharacterController>();
        thirdPersonController._mainCamera = sceneCam;
        thirdPersonController.CinemachineCameraTarget = camLockGo;
    }
}

