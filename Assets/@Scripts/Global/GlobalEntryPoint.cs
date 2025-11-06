using ComBots.Global.UI;
using ComBots.Inputs;
using System;
using UnityEngine;

namespace ComBots.Global
{
    /// <summary>
    /// Soon to be deprecated, JC 2025-11-05
    /// </summary>
    [Obsolete]
    public class GlobalEntryPoint : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }

        [SerializeField] private InputManager _inputManager;

        void Start()
        {
            _inputManager.TryInit();

            // Set as initialized
            IsInitialized = true;
        }

        void OnDestroy()
        {
            IsInitialized = false;
        }
    }
}