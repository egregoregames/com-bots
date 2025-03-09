using System;
using UnityEngine;

namespace Game.src
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
            uiSo.TriggerAreaChangeTransition += OnRoomTransition;
            uiSo.PlayerEnteredRoomSelector += OnPlayerEnteredSelectionPortal;

            uiSo.OnPushDialogue += OnDialogue;
        }

        public void OnDialogue(string[] dialogue)
        {
            menuController.HandleDialogue(dialogue);
        }
        

        public void OnPlayerEnteredSelectionPortal(Room[] rooms, Action<Room> roomSelected, string cancelText)
        {
            menuController.RoomSelectPanel(rooms, roomSelected, cancelText);
        }

        public void OnRoomTransition(Action onTransitionStart, Action onTransitionEnd, string areaName)
        {
            
        }
    }
}
