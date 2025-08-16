using ComBots.Global.UI;
using ComBots.Global.UI.Dialogue;
using ComBots.Inputs;
using ComBots.Utils.StateMachines;
using UnityEngine.Events;
using static ComBots.Game.StateMachine.GameStateMachine;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_Dialogue : State
        {
            private readonly GameStateMachine _stateMachine;

            public State_Dialogue(GameStateMachine stateMachine) : base("Dialogue")
            {
                _stateMachine = stateMachine;
            }

            public override bool Enter(State previousState, object args = null)
            {
                bool canEnter = true;
                if (previousState is not State_Playing)
                {
                    canEnter = false;
                }
                if (args == null || args is not State_Dialogue_Args)
                {
                    canEnter = false;
                }

                if (canEnter)
                {
                    GlobalUIRefs.I.DialogueController.SetActive((State_Dialogue_Args)args);
                    // Hide the menu's bottom bar
                    GlobalUIRefs.I.MenuController.SetBottomBarVisible(false);
                    // Push the dialogue input context
                    InputManager.I.PushContext(_stateMachine._dialogueContextData, DialogueInputHandler.I);
                }

                return canEnter;
            }

            public override bool Exit(State nextState)
            {
                // Pop the dialogue input context
                InputManager.I.PopContext(_stateMachine._dialogueContextData.contextName);
                // Inform the dialogue controller of state exit & deactivate it
                GlobalUIRefs.I.DialogueController.OnExit();
                GlobalUIRefs.I.DialogueController.SetInactive();
                // Display back the menu bottom bar
                GlobalUIRefs.I.MenuController.SetBottomBarVisible(true);

                return true;
            }
        }
    }


    public class State_Dialogue_Args
    {
        public string Dialogue;
        public string Nametag;
        public DialogueOptions OptionsArgs;

        public State_Dialogue_Args(string dialogue, string nametag, DialogueOptions options)
        {
            Dialogue = dialogue;
            Nametag = nametag;
            OptionsArgs = options;
        }

        public class DialogueOptions
        {
            public string[] Options;
            public string CancelOption;
            public UnityAction<int> Callback;

            public DialogueOptions(string[] options, string cancelOption, UnityAction<int> callback)
            {
                Options = options;
                CancelOption = cancelOption;
                Callback = callback;
            }
        }
    }
}