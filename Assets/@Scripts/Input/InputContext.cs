using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

namespace ComBots.Inputs
{
    public class InputContext
    {
        public string Name { get; private set; }
        public IInputHandler Handler { get; private set; }
        public InputFlags AllowedInputs { get; private set; }
        public bool BlockLowerContexts { get; private set; }
        public bool Persistent { get; private set; }

        private readonly InputContextConfig _contextConfig;
        private readonly Dictionary<string, InputFlags> _actionToFlagMap;
        private readonly List<InputActionMap> _activeActionMaps;
        private readonly List<InputAction> _allActions;

        public InputContext(InputContextConfig config, IInputHandler inputHandler)
        {
            _contextConfig = config;
            Name = config.contextName;
            Handler = inputHandler;
            AllowedInputs = config.allowedInputs;
            BlockLowerContexts = config.blockLowerContexts;
            Persistent = config.persistent;

            _activeActionMaps = new List<InputActionMap>();
            _actionToFlagMap = new Dictionary<string, InputFlags>();
            _allActions = new List<InputAction>();

            SetupInputActions();
        }

        private void SetupInputActions()
        {
            if (_contextConfig.inputActions == null) return;

            var activeMaps = _contextConfig.GetActiveActionMaps(AllowedInputs);

            foreach (var actionMap in activeMaps)
            {
                _activeActionMaps.Add(actionMap);

                // Map each action to its corresponding flag and collect all actions
                foreach (var action in actionMap.actions)
                {
                    _allActions.Add(action);
                    
                    var flag = GetFlagForAction(action.name);
                    if (flag != InputFlags.None)
                    {
                        _actionToFlagMap[action.name] = flag;
                    }
                }
            }
        }

        private static InputFlags GetFlagForAction(string actionName)
        {
            // Map action names to flags - you can customize this based on your naming convention
            return actionName.ToLower() switch
            {
                // Movement actions
                "move" or "movement" or "walk" => InputFlags.Movement,
                "speed" or "run" or "speedboost" => InputFlags.Speed,

                // World interaction
                "interact" or "use" => InputFlags.Interaction,

                // Camera
                "look" or "camera" or "mouselook" => InputFlags.Camera,

                // UI Navigation
                "navigate" or "uinavigate" => InputFlags.UI,

                // Confirmation/Cancellation
                "confirm" or "submit" or "select" => InputFlags.Confirm,
                "cancel" or "back" => InputFlags.Cancel,

                // Combat
                "combatmove" or "combatnavigate" => InputFlags.Combat,
                "attack" or "defend" => InputFlags.Combat,

                // Global
                "pause" or "pausegame" => InputFlags.Pause | InputFlags.Global,

                _ => InputFlags.None
            };
        }

        public void EnableActions()
        {
            foreach (var actionMap in _activeActionMaps)
            {
                actionMap.Enable();
            }
        }

        public void DisableActions()
        {
            foreach (var actionMap in _activeActionMaps)
            {
                actionMap.Disable();
            }
        }

        public bool CanHandleAction(string actionName)
        {
            if (!_actionToFlagMap.TryGetValue(actionName, out var flag))
                return false;

            return (AllowedInputs & flag) != 0;
        }

        public InputFlags GetActionFlag(string actionName)
        {
            return _actionToFlagMap.TryGetValue(actionName, out var flag) ? flag : InputFlags.None;
        }

        // New method needed by InputManager
        public IEnumerable<InputAction> GetAllActions()
        {
            return _allActions;
        }
    }
}