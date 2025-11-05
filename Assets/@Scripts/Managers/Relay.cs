using System;
using ComBots.Game.Worlds.Rooms;
using UnityEngine;

namespace ComBots.src
{
    public class Relay : MonoBehaviour
    {
        [SerializeField] InputSO inputSo;
        [SerializeField] UISo uiSo;
        [SerializeField] StageDoorTransitions stageDoorTransitions;
        [SerializeField] ChatBubble chatBubble;

        // public void OnPlayerEnteredSelectionPortal(Room[] rooms, Action<Room> roomSelected, string cancelText, string optionMessage)
        // {
        //     menuController.RoomSelectPanel(rooms, roomSelected, cancelText, optionMessage);
        // }
    }
}
