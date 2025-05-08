using System;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;
    public GameObject camLock;
    public GameObject cameraPlayer;
    public GameObject sceneCam;
    private void Start()
    {
        Spawn();
    }


    [ContextMenu("Spawn")]
    public void Spawn()
    {
        var playerGo = (GameObject)Instantiate(player, transform.position, Quaternion.identity);
        var camGo = (GameObject)Instantiate(cameraPlayer, transform.position, Quaternion.identity);
        var camLockGo = (GameObject)Instantiate(camLock, transform.position, Quaternion.identity);
        
        var sceneCam = (GameObject)Instantiate(this.sceneCam, transform.position, Quaternion.identity);

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

