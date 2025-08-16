using UnityEngine.InputSystem;

namespace ComBots.Inputs
{
    public interface IInputHandler
    {
        bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag);
        void OnInputContextEntered(InputContext context);
        void OnInputContextExited(InputContext context);
        void OnInputContextPaused(InputContext context);
        void OnInputContextResumed(InputContext context);
    }
}