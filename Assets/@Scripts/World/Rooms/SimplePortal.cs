using ComBots.Game.Players;
using ComBots.Game.StateMachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComBots.Game.Portals
{
    public class SimplePortal : Portal
    {
        public Portal nextPortal;
        public AudioClip clip;
        public string areaName = "AREA NAME NOT SET";
        protected override void OnPlayerEnter(Player playerThatEntered)
        {
            OnTriggerPortal();
        }
        
        private void OnTriggerPortal()
        {
            Player.InputSO.CanPlayerMove = false;
            State_AreaTransition_ToPortal_Args args = new(nextPortal, Player, areaName, clip);
            GameStateMachine.I.SetState<GameStateMachine.State_AreaTransition>(args);
        }
    }
}