using ComBots.Cameras;
using ComBots.Game.Players;
using ComBots.Global.UI;
using ComBots.Global.UI.Dialogue;
using ComBots.Inputs;
using ComBots.Signs;
using ComBots.Utils.StateMachines;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;
using static ComBots.Game.StateMachine.GameStateMachine;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_Sign : State
        {
            private readonly GameStateMachine _stateMachine;
            private State_Sign_Args _args;

            public State_Sign(GameStateMachine stateMachine) : base("Sign")
            {
                _stateMachine = stateMachine;
            }

            public override bool Enter(State previousState, object args = null)
            {
                bool canEnter = true;
                if (!(previousState is State_Playing || previousState is State_Dialogue))
                {
                    canEnter = false;
                }
                if (args == null || args is not State_Sign_Args)
                {
                    canEnter = false;
                }

                if (canEnter)
                {
                    _args = (State_Sign_Args)args;
                    GlobalUIRefs.I.SignController.SetActive(_args.SignText);
                    // Hide the menu's bottom bar
                    // GlobalUIRefs.I.MenuController.SetBottomBarVisible(false);
                    // Push the dialogue input context
                    InputManager.I.PushContext(_stateMachine._signContextData, SignInputHandler.I);
                }

                return canEnter;
            }

            public override bool Exit(State nextState)
            {
                // Pop the dialogue input context
                InputManager.I.PopContext(_stateMachine._dialogueContextData.contextName);
                // Inform the dialogue controller of state exit & deactivate it
                GlobalUIRefs.I.SignController.SetInactive();
                // Display back the menu bottom bar
                //GlobalUIRefs.I.MenuController.SetBottomBarVisible(true);
                _args.OnStateExited?.Invoke();

                return true;
            }
        }
    }

    public class State_Sign_Args
    {
        public string SignText { get; }
        public UnityAction OnStateExited { get; }

        public State_Sign_Args(string signText, UnityAction OnStateExited)
        {
            SignText = signText;
            this.OnStateExited = OnStateExited;
        }
    }
}