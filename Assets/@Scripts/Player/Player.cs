using System.Collections;
using ComBots.Cameras;
using ComBots.Game.Interactions;
using PixelCrushers.DialogueSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace ComBots.Game.Players
{
    public class Player : MonoBehaviour, IInteractor
    {
        public static Player I { get; private set; }

        private bool _isInitialized = false;

        // Tags
        public const string TAG_TRIGGER = "Trigger";

        // References
        [SerializeField] private Transform T;
        Transform IInteractor.T => T;

        [Header("Camera")]
        [SerializeField] private PlayerCamera _playerCamera;
        public PlayerCamera PlayerCamera => _playerCamera;

        [Header("Input")]
        [SerializeField] private PlayerInputHandler _inputHandler;
        public PlayerInputHandler InputHandler => _inputHandler;
        [SerializeField] private InputSO _inputSO;
        public InputSO InputSO => _inputSO;

        [Header("Interactions")]
        [SerializeField] private PlayerInteractor _interactor;
        public PlayerInteractor Interactor => _interactor;

        [Header("Movement")]
        [SerializeField] private StarterAssets.ThirdPersonController _thirdPersonController;
        public StarterAssets.ThirdPersonController Controller => _thirdPersonController;

        [Header("Dialogue")]
        [SerializeField] private DialogueActor _dialogueActor;
        public DialogueActor DialogueActor => _dialogueActor;

        public IInteractor.InteractorState State => _interactor.State;

        void Awake()
        {
            TryInit();
        }

        private void TryInit()
        {
            if (_isInitialized) { return; }

            I = this;
            _inputHandler.TryInit();
            _interactor.TryInit();

            _isInitialized = true;
        }

        void OnDestroy()
        {
            if (I == this)
            {
                I = null;
            }
        }

        public void OnInteractionStart(IInteractable interactable)
        {
            _interactor.OnInteractionStart(interactable);
        }

        public void OnInteractionEnd(IInteractable interactable)
        {
            _interactor.OnInteractionEnd(interactable);
        }

        public void FreezeMovementFor(float seconds)
        {
            StartCoroutine(FreezeMovementCoroutine(seconds));
        }

        private IEnumerator FreezeMovementCoroutine(float seconds)
        {
            _thirdPersonController.FreezeMovement = true;
            yield return new WaitForSeconds(seconds);
            _thirdPersonController.FreezeMovement = false;
        }
    }
}