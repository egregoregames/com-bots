using System.Collections;
using ComBots.Game.Players;
using ComBots.Global.UI;
using ComBots.Global.UI.Dialogue;
using ComBots.Global.UI.Menu;
using ComBots.Inputs;
using UnityEngine;

namespace ComBots.Global
{
    /// <summary>
    /// Soon to be deprecated, JC 2025-11-05
    /// </summary>
    public class GlobalEntryPoint : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }

        [Header("Input")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private DialogueInputHandler _dialogueInputHandler;
        //[SerializeField] private MenuInputHandler _menuInputHandler;

        [Header("UI")]
        [SerializeField] private GlobalUIRefs _globalUIRefs;

        void Start()
        {
            //// Independent initialization
            // UI
            _globalUIRefs.TryInit();
            // Input
            _inputManager.TryInit();
            _dialogueInputHandler.TryInit();
            //_menuInputHandler.TryInit();
            
            //// Dependent initialization

            // Set as initialized
            IsInitialized = true;
        }

        void OnDestroy()
        {
            IsInitialized = false;
        }
    }
}