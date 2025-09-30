using ComBots.Game.Players;
using ComBots.Game.StateMachine;
using ComBots.Game.Worlds.Rooms;
using ComBots.Logs;
using StarterAssets;
using UnityEngine;

namespace ComBots.Game.Portals
{
    public class SelectionPortal : Portal
    {
        public SelectionPortal nextPortal;
        public UISo uiSo;
        public Room[] rooms;
        public string cancelText;
        public string optionMessage;

        private Room _roomSelected;

        protected override void OnPlayerEnter(Player player)
        {
            Player = player;
            string[] options = new string[rooms.Length];
            for (int i = 0; i < rooms.Length; i++)
            {
                options[i] = rooms[i].optionName;
            }
            DialogueOptions dialogueOptions = new (options, cancelText, OnRoomSelected);
            State_Dialogue_Args args = new (optionMessage, null, dialogueOptions);
            GameStateMachine.I.SetState<GameStateMachine.State_Dialogue>(args);
        }

        private void OnRoomSelected(int index)
        {
            MyLogger<SelectionPortal>.StaticLog($"Room selection index: {index}");
            _roomSelected = index != -1 ? rooms[index] : null;
            if (_roomSelected != null)
            {
                MyLogger<SelectionPortal>.StaticLog($"Selected room: {_roomSelected.name}");
                State_AreaTransition_ToRoom_Args args = new(_roomSelected, Player);
                GameStateMachine.I.SetState<GameStateMachine.State_AreaTransition>(args);
            }
            else
            {
                MyLogger<SelectionPortal>.StaticLog("No room selected, will set state to playing.");
                GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
            }
            Player = null;
        }
    }
}