using System.Collections;
using ComBots.Game.Players;
using ComBots.Global.Audio;
using ComBots.Global.UI;
using ComBots.Global.UI.Dialogue;
using ComBots.Global.UI.Menu;
using ComBots.Inputs;
using UnityEngine;

namespace ComBots.Global
{
    public class GlobalEntryPoint : MonoBehaviour
    {
        public static bool IsInitialized { get; private set; }

        [Header("Input")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private DialogueInputHandler _dialogueInputHandler;
        //[SerializeField] private MenuInputHandler _menuInputHandler;

        [Header("UI")]
        [SerializeField] private GlobalUIRefs _globalUIRefs;

        [Header("Audio")]
        [SerializeField] private AudioManager _audioManager;

        void Start()
        {
            //// Independent initialization
            // Audio
            _audioManager.TryInit();
            // UI
            _globalUIRefs.TryInit();
            // Input
            _inputManager.TryInit();
            //_dialogueInputHandler.TryInit();
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