using ComBots.Inputs;
using ComBots.Logs;
using ComBots.Utils.EntryPoints;
using ComBots.Utils.StateMachines;
using UnityEngine;

namespace ComBots.Game.StateMachine
{
    public partial class GameStateMachine : MonoStateMachine<GameStateMachine>
    {
        public static GameStateMachine I { get; private set; }

        private State STATE_Playing, STATE_Dialogue, STATE_AreaTransition;

        public override Dependency Dependency => Dependency.Dependent;

        [Header("Input Contexts")]
        [SerializeField] private InputContextConfig _overworldContextData;
        [SerializeField] private InputContextConfig _pauseContextData;
        [SerializeField] private InputContextConfig _dialogueContextData;
        [SerializeField] private InputContextConfig _signContextData;

        protected override State[] InitStates(out State initialState)
        {
            STATE_Playing = new State_Playing(this);
            STATE_Dialogue = new State_Dialogue(this);
            STATE_AreaTransition = new State_AreaTransition(this);

            initialState = STATE_Playing;
            return new[] { STATE_Playing, STATE_Dialogue, STATE_AreaTransition };
        }

        protected override void Init()
        {
            base.Init();
            I = this;
        }

        public override void Dispose()
        {
            base.Dispose();
            I = null;
        }

        public void SetState<StateT>(object args)
        {
            foreach (var state in States)
            {
                if (state is StateT)
                {
                    SetState(state, args);
                    return;
                }
            }
        }

        public bool ExitState<StateT>()
        {
            if (CurrentState is not StateT)
            {
                _logger.LogError($"Could not exit state {CurrentState} as it is not the current state.");
                return false;
            }
            if (!SetState(STATE_Playing, null))
            {
                _logger.LogError($"Could not transition from state {CurrentState} to state Playing");
                return false;
            }
            return true;
        }
    }
}