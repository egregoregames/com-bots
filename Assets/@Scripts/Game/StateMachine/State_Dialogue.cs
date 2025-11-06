using ComBots.Cameras;
using ComBots.Game.Players;
using ComBots.Global.UI;
using ComBots.Inputs;
using ComBots.Utils.StateMachines;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_Dialogue : State
        {
            private readonly GameStateMachine _stateMachine;
            private object _args;

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
                if (args == null || args is not IState_Dialogue_Args)
                {
                    canEnter = false;
                }

                if (canEnter)
                {
                    _args = args;
                    WC_Dialogue.Instance.SetActive((IState_Dialogue_Args)args);
                    // Hide the menu's bottom bar
                    //GlobalUIRefs.I.MenuController.SetBottomBarVisible(false);
                    // Push the dialogue input context
                    InputManager.I.PushContext(_stateMachine._dialogueContextData, DialogueInputHandler.I);
                    // Camera
                    if (args is State_Dialogue_PixelCrushers_Args pcArgs)
                    {
                        CameraTarget cameraTarget = pcArgs.CameraTarget == null ? Player.I.PlayerCamera.CameraTarget : pcArgs.CameraTarget;
                        Player.I.PlayerCamera.SetState_Dialogue(cameraTarget);
                    }
                }

                return canEnter;
            }

            public override bool Exit(State nextState)
            {
                if(_args != null && _args is State_Dialogue_PixelCrushers_Args pcArgs)
                {
                    pcArgs.OnDialogueEnd?.Invoke();
                }
                // Pop the dialogue input context
                InputManager.I.PopContext(_stateMachine._dialogueContextData.contextName);
                // Inform the dialogue controller of state exit & deactivate it

                // THESE ARE NOT EVENTS
                WC_Dialogue.Instance.OnExit();
                WC_Dialogue.Instance.SetInactive();
                // Display back the menu bottom bar
                //GlobalUIRefs.I.MenuController.SetBottomBarVisible(true);
                Player.I.PlayerCamera.SetState_Orbital();

                return true;
            }
        }
    }

    public interface IState_Dialogue_Args
    {
        public string Nametag { get; }
    }

    public class State_Dialogue_Args : IState_Dialogue_Args
    {
        public string Dialogue { get; private set; }
        public string Nametag { get; private set; }
        public DialogueOptions OptionsArgs;
        public Animator Animator { get; private set; }

        public State_Dialogue_Args(string dialogue, string nametag, DialogueOptions options)
        {
            Dialogue = dialogue;
            Nametag = nametag;
            OptionsArgs = options;
        }
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

    public class State_Dialogue_PixelCrushers_Args : IState_Dialogue_Args
    {
        public DialogueActor Actor { get; private set; }
        public DialogueActor Conversant { get; private set; }
        public CameraTarget CameraTarget { get; private set; }
        public UnityAction OnDialogueEnd;
        public string Nametag => Actor.GetActorName();
        public string ConversationTitle { get; private set; }
        public UnityAction<string> PlayConversantAnimation { get; private set; }
        public UnityAction<string> PlayActorAnimation { get; private set; }

        public State_Dialogue_PixelCrushers_Args(string conversationTitle, DialogueActor actor, DialogueActor conversant, CameraTarget cameraTarget, UnityAction<string> playActorAnimation, UnityAction<string> playConversantAnimation, UnityAction onDialogueEnd)
        {
            ConversationTitle = conversationTitle;
            Actor = actor;
            Conversant = conversant;
            CameraTarget = cameraTarget;
            PlayConversantAnimation = playConversantAnimation;
            PlayActorAnimation = playActorAnimation;
            OnDialogueEnd = onDialogueEnd;
        }
    }
}