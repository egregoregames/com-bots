using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Portal Portal;
    public GameObject GameObject => gameObject;
    public AudioClip clip;

    public void TeleportPlayerToRoom(GameObject player)
    {
        Portal.player = player;
        Portal.SpawnPlayerAtPortal();
    }
    
}
