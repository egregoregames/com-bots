using System;
using ComBots.Game.Players;
using ComBots.Game.Portals;
using ComBots.Logs;
using UnityEngine;

namespace ComBots.Game.Worlds.Rooms
{
    public class Room : MonoBehaviour
    {
        public Portal Portal;
        public AudioClip clip;
        public string optionName;
        public string bannerName;

        public void TeleportPlayerToRoom(Player player)
        {
            Portal.TeleportPlayer(player);
            MyLogger<Room>.StaticLog($"Teleported player<{player.name}> to room<{optionName}>");
        }
    }
}