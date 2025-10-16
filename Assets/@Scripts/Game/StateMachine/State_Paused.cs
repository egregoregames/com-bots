using ComBots.Global.UI;
using ComBots.Inputs;
using ComBots.Sandbox.Global.UI.Menu;
using ComBots.Utils.StateMachines;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_Paused : State
        {
            private readonly GameStateMachine _stateMachine;

            public State_Paused(GameStateMachine stateMachine) : base("Paused")
            {
                _stateMachine = stateMachine;
            }

            public override bool Enter(State previousState, object args = null)
            {
                bool canEnter = previousState is State_Playing;
                if (canEnter)
                {
                    InputManager.I.PushContext(_stateMachine._pauseContextData, PauseMenu.Instance);
                }
                return canEnter;
            }

            public override bool Exit(State nextState)
            {
                bool canExit = true;
                if (canExit)
                {
                    InputManager.I.PopContext(_stateMachine._pauseContextData.contextName);
                }
                return canExit;
            }
        }
    }
}