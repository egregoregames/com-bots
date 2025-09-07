using ComBots.Inputs;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class MenuInputHandler : EntryPointMono, IInputHandler
    {
        public static MenuInputHandler I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [SerializeField] private MenuController _menuController;
        [SerializeField] private MenuNavigationController _navigationController;

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
            switch (actionName)
            {
                case "pause":
                case "cancel":
                    _menuController.HandleInput(context, actionName, inputFlag);
                    return true;
            }
            return _navigationController.HandleInput(context, actionName, inputFlag);
        }

        public void OnInputContextEntered(InputContext context) { }
        public void OnInputContextExited(InputContext context) { }
        public void OnInputContextPaused(InputContext context) { }
        public void OnInputContextResumed(InputContext context) { }
    }
}