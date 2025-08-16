using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.Utils.EntryPoints;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.UI.Game.Players
{
    public class PlayerInputHandler : EntryPointMono, IInputHandler
    {
        public static PlayerInputHandler I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [SerializeField] private ThirdPersonController _controller;

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case "pause":
                    if (context.phase == InputActionPhase.Performed)
                    {
                        GameStateMachine.I.SetState<GameStateMachine.State_Paused>(null);
                    }
                    return true;
            }
            return _controller.HandleInput(context, actionName, inputFlag);
        }

        public void OnInputContextEntered(InputContext context)
        {
            _controller.OnInputContextEntered(context);
        }

        public void OnInputContextExited(InputContext context)
        {
            _controller.OnInputContextExited(context);
        }

        public void OnInputContextPaused(InputContext context)
        {
            _controller.OnInputContextPaused(context);
        }

        public void OnInputContextResumed(InputContext context)
        {
            _controller.OnInputContextResumed(context);
        }

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
            _controller = null;
        }
    }
}