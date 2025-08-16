using ComBots.Logs;
using ComBots.UI.Game.Players;
using UnityEngine;

namespace ComBots.Game.Players
{
    public class Player : MonoBehaviour
    {
        public static Player I { get; private set; }

        private bool _isInitialized = false;

        // Tags
        public const string TAG_TRIGGER = "Trigger";

        // References
        [SerializeField] private Transform T;

        [Header("Input")]
        [SerializeField] private PlayerInputHandler _inputHandler;
        public PlayerInputHandler InputHandler => _inputHandler;
        [SerializeField] private InputSO _inputSO;
        public InputSO InputSO => _inputSO;

        [Header("Movement")]
        [SerializeField] private StarterAssets.ThirdPersonController _thirdPersonController;
        public StarterAssets.ThirdPersonController Controller => _thirdPersonController;

        void Awake()
        {
            TryInit();
        }

        private void TryInit()
        {
            if (_isInitialized) { return; }

            I = this;
            _inputHandler.TryInit();

            _isInitialized = true;
        }

        void OnDestroy()
        {
            if (I == this)
            {
                I = null;
            }
        }
    }
}