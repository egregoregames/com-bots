using System;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Portal Portal;
    public AudioClip clip;
    public string optionName;
    public string bannerName;

    public void TeleportPlayerToRoom(GameObject player)
    {
        Portal.player = player;
        Portal.SpawnPlayerAtPortal();
    }
    
}
