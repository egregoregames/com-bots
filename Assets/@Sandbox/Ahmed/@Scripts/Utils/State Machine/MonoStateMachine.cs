using UnityEngine;
using UnityEngine.Events;
using ComBots.Utils.EntryPoints;
using ComBots.Logs;

namespace ComBots.Utils.StateMachines
{
    public abstract class MonoStateMachine<ChildT> : EntryPointMono where ChildT : MonoStateMachine<ChildT>
    {
        protected State[] States { get; private set; }
        private int _currentStateIndex = -1;
        protected State CurrentState => IsStateMachineInitialized ? States[_currentStateIndex] : null;

        protected bool IsStateMachineInitialized { get; private set; } = false;

        [Header("Logs")]
        [SerializeField] protected MyLogger<ChildT> _logger;

        protected abstract State[] InitStates(out State initialState);

        protected override void Init()
        {
            _logger.TryInit();
            IsStateMachineInitialized = false;
            InitStateMachine();
        }

        public override void Dispose()
        {
            States = null;
        }

        private void InitStateMachine()
        {
            States = InitStates(out State initialState);
            if (States == null || States.Length == 0)
            {

                _logger.LogError("State machine must have at least one state defined.");
                return;
            }
            _currentStateIndex = GetStateIndex(initialState);
            if (_currentStateIndex == -1)
            {
                _logger.LogError($"Initial state '{initialState.Name}' is not defined in the state machine.");
                States = null;
                return;
            }
            initialState.Enter(null, null);
            IsStateMachineInitialized = true;
        }

        protected bool SetState(State newState, object args)
        {
            if (!IsStateMachineInitialized)
            {
                _logger.LogError("State machine is not initialized. Call Init() before setting a state.");
                return false;
            }

            State previousState = CurrentState;
            if (previousState != null && !previousState.Exit(newState))
            {
                _logger.LogError($"Failed to exit state '{previousState.Name}'. Cannot transition to '{newState.Name}'.");
                return false;
            }
            if (!newState.Enter(previousState, args))
            {
                _logger.LogError($"Failed to enter state '{newState.Name}'. Cannot transition from '{previousState?.Name ?? "null"}'.");
                return false;
            }
            _currentStateIndex = GetStateIndex(newState);
            return true;
        }

        private int GetStateIndex(State state)
        {
            for (int i = 0; i < States.Length; i++)
            {
                if (States[i] == state)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}