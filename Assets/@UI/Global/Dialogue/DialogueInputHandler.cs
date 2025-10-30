using ComBots.Inputs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Global.UI.Dialogue
{
    public class DialogueInputHandler : EntryPointMono, IInputHandler
    {
        public const string INPUT_ACTION_NAVIGATE = "navigate";
        public const string INPUT_ACTION_CANCEL = "cancel";
        public const string INPUT_ACTION_CONFIRM = "confirm";

        public static DialogueInputHandler I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [SerializeField] private WC_Dialogue _dialogueController;

        protected override void Init()
        {
            I = this;
        }

        public override void Dispose()
        {
            if (I == this)
            {
                I = null;
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            return _dialogueController.HandleInput(context, actionName, inputFlag);
        }

        public void OnInputContextEntered(InputContext context)
        {
        }

        public void OnInputContextExited(InputContext context)
        {
        }

        public void OnInputContextPaused(InputContext context)
        {
        }

        public void OnInputContextResumed(InputContext context)
        {
        }
    }
}