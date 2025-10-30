using ComBots.Global.UI;
using ComBots.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Signs
{
    public class SignInputHandler : MonoBehaviour, IInputHandler
    {
        // =============== Input Mappings =============== //
        public const string INPUT_ACTION_CANCEL = "cancel";
        public const string INPUT_ACTION_CONFIRM = "confirm";
        // =============== Singleton =============== //
        public static SignInputHandler I { get; private set; }

        #region Unity Lifecycle
        // ----------------------------------------
        // Unity Lifecycle 
        // ----------------------------------------

        void Awake()
        {
            I = this;
        }

        #endregion

        #region IInputHandler Interface
        // ----------------------------------------
        // IInputHandler Interface 
        // ----------------------------------------

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case INPUT_ACTION_CONFIRM:
                    if (context.performed)
                    {
                        GlobalUIRefs.I.SignController.Input_Dismiss();
                    }
                    return true;
                case INPUT_ACTION_CANCEL:
                    if (context.performed)
                    {
                        GlobalUIRefs.I.SignController.Input_Dismiss();
                    }
                    return true;
            }
            return false;
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

        #endregion
    }
}