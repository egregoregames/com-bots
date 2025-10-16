using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.Utils.EntryPoints;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Game.Players
{
    public class PlayerInputHandler : EntryPointMono, IInputHandler
    {
        public static PlayerInputHandler I { get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [Header("Player")]
        [SerializeField] private Player _player;

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case "pause":
                    if (context.phase == InputActionPhase.Performed)
                    {
                        
                        if (GameStateMachine.I.CurrentState is GameStateMachine.State_Paused)
                        {
                            Debug.Log("Pausing game...");
                            GameStateMachine.I.SetState<GameStateMachine.State_Paused>(null);
                        }
                        else
                        {
                            Debug.Log("Unpausing game...");
                            GameStateMachine.I.ExitState<GameStateMachine.State_Paused>();
                        }
                    }
                    return true;
            }
            if (_player.Interactor.HandleInput(context, actionName, inputFlag))
            {
                return true;
            }
            if (_player.Controller.HandleInput(context, actionName, inputFlag))
            {
                return true;
            }
            return false;
        }

        public void OnInputContextEntered(InputContext context)
        {
            _player.Controller.OnInputContextEntered(context);
        }

        public void OnInputContextExited(InputContext context)
        {
            _player.Controller.OnInputContextExited(context);
        }

        public void OnInputContextPaused(InputContext context)
        {
            _player.Controller.OnInputContextPaused(context);
        }

        public void OnInputContextResumed(InputContext context)
        {
            _player.Controller.OnInputContextResumed(context);
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
            _player = null;
        }
    }
}