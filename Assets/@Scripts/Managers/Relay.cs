using System;
using ComBots.Game.Worlds.Rooms;
using UnityEngine;

namespace ComBots.src
{
    public class Relay : MonoBehaviour
    {
        [SerializeField] InputSO inputSo;
        [SerializeField] UISo uiSo;
        [SerializeField] MenuController menuController;
        [SerializeField] StageDoorTransitions stageDoorTransitions;
        [SerializeField] ChatBubble chatBubble;

        private void Awake()
        {
            //uiSo.PlayerEnteredRoomSelector += OnPlayerEnteredSelectionPortal;

            uiSo.OnPushDialogue += OnDialogue;
        }

        public void OnDialogue(string[] dialogue)
        {
            menuController.HandleDialogue(dialogue);
        }
        

        // public void OnPlayerEnteredSelectionPortal(Room[] rooms, Action<Room> roomSelected, string cancelText, string optionMessage)
        // {
        //     menuController.RoomSelectPanel(rooms, roomSelected, cancelText, optionMessage);
        // }
    }
}
