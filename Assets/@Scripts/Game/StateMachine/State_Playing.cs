using ComBots.Game.Players;
using ComBots.Inputs;
using ComBots.Utils.StateMachines;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public class State_Playing : State
        {
            private readonly GameStateMachine _stateMachine;

            public State_Playing(GameStateMachine stateMachine) : base("Playing")
            {
                _stateMachine = stateMachine;
            }

            public override bool Enter(State previousState, object args = null)
            {
                Player.I.PlayerCamera.SetState_Orbital();
                Player.I.FreezeMovementFor(Player.I.PlayerCamera.BlendTime);
                InputManager.I.PushContext(_stateMachine._overworldContextData, PlayerInputHandler.I);
                return true;
            }

            public override bool Exit(State nextState)
            {
                InputManager.I.PopContext(_stateMachine._overworldContextData.contextName);
                return true;
            }
        }
    }
}