using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Inputs
{
    public abstract class BaseInputHandler : MonoBehaviour, IInputHandler
    {
        protected InputContext currentContext;

        public virtual bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            return false; // Override in derived classes
        }

        public virtual void OnInputContextEntered(InputContext context)
        {
            currentContext = context;
        }

        public virtual void OnInputContextExited(InputContext context)
        {
            currentContext = null;
        }

        public virtual void OnInputContextPaused(InputContext context) { }
        public virtual void OnInputContextResumed(InputContext context) { }
    }
}