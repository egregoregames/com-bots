using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComBots.Inputs
{
    [CreateAssetMenu(fileName = "InputContext", menuName = "ComBots/Input/Input Context Config")]
    public class InputContextConfig : ScriptableObject
    {
        [Header("Context Settings")]
        public string contextName;
        public InputFlags allowedInputs = InputFlags.All;
        public bool blockLowerContexts = true;
        public bool persistent = false;

        [Header("Input Actions")]
        public InputActionAsset inputActions;

        [Header("Action Map Assignments")]
        public InputActionMapAssignment[] actionMaps;

        [System.Serializable]
        public class InputActionMapAssignment
        {
            public string mapName;
            public InputFlags requiredFlags;

            [HideInInspector] public InputActionMap actionMap;
        }

        private void OnEnable()
        {
            if (inputActions != null)
            {
                // Cache action map references
                foreach (var assignment in actionMaps)
                {
                    assignment.actionMap = inputActions.FindActionMap(assignment.mapName);
                }
            }
        }

        public InputActionMap[] GetActiveActionMaps(InputFlags currentFlags)
        {
            List<InputActionMap> activeMaps = new();

            foreach (var assignment in actionMaps)
            {
                if ((assignment.requiredFlags & currentFlags) != 0)
                {
                    activeMaps.Add(assignment.actionMap);
                }
            }

            return activeMaps.ToArray();
        }
    }
}