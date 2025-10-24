using ComBots.Inputs;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using ComBots.Game.Tags;
using PixelCrushers.DialogueSystem;
using ComBots.Game.Interactions;

namespace ComBots.Game.Players
{
    public class PlayerInteractor : EntryPointMono, IInputHandler
    {
        private const string INPUT_INTERACT = "interact";
        public override Dependency Dependency => Dependency.Independent;

        [Header("Player")]
        [SerializeField] private Player _player;
        [SerializeField] private Transform _T;
        [SerializeField] private float _interactionRadius;
        private IInteractable _closestInteractable;

        public IInteractor.InteractorState State { get; private set; }

        [Header("Settings")]
        [SerializeField, Min(0)] private int _checkRate;
        private int _checkCounter = 0;

        protected override void Init()
        {
        }

        public override void Dispose()
        {

        }

        void OnDrawGizmosSelected()
        {
            if (_T == null) { return; }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(_T.position, _interactionRadius);
        }

        void Update()
        {
            if (State == IInteractor.InteractorState.Interacting) { return; }
            // Check rate limiting
            if (_checkCounter < _checkRate)
            {
                _checkCounter++;
                return;
            }
            _checkCounter = 0;
            // Check for nearby interactables
            var colliders = Physics.OverlapSphere(_T.position, _interactionRadius);
            if (colliders.Length == 0) { return; }
            // Figure out the closest interactable
            IInteractable interactable = null;
            float closestDistance = float.MaxValue;
            foreach (var collider in colliders)
            {
                if (!collider.TryGetComponent<IInteractable>(out var k)) { continue; }
                float distance = Vector3.Distance(_T.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    interactable = k;
                }
            }
            // Notify if the closest interactable changed
            if (interactable != _closestInteractable)
            {
                if (_closestInteractable != null)
                {
                    _closestInteractable.OnInteractorFar(_player);
                }
                if (interactable != null)
                {
                    interactable.OnInteractorNearby(_player);
                }
                _closestInteractable = interactable;
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case INPUT_INTERACT:
                    if (context.phase == InputActionPhase.Performed)
                    {
                        var colliders = Physics.OverlapSphere(_T.position, _interactionRadius);
                        if (colliders.Length == 0) { return false; }

                        // Figure out the closest interactable
                        IInteractable interactable = null;
                        float closestDistance = float.MaxValue;
                        foreach (var collider in colliders)
                        {
                            if (!collider.TryGetComponent<IInteractable>(out var k)) { continue; }
                            float distance = Vector3.Distance(_T.position, collider.transform.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                interactable = k;
                            }
                        }
                        if (interactable != null && InteractionManager.I.StartInteraction(_player, interactable))
                        {
                            Quaternion targetRotation = Quaternion.LookRotation(interactable.T.position - _T.position);
                            Vector3 euler = targetRotation.eulerAngles;
                            targetRotation = Quaternion.Euler(0, euler.y, 0);
                            _player.Controller.SetRotation(targetRotation);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }

        public void OnInputContextEntered(InputContext context)
        {
        }

        public void OnInputContextExited(InputContext context)
        {
        }

        public void OnInputContextPaused(InputContext context)
        {
        }

        public void OnInputContextResumed(InputContext context)
        {
        }

        public void OnInteractionStart(IInteractable interactable)
        {
            State = IInteractor.InteractorState.Interacting;
            _closestInteractable = null;
        }

        public void OnInteractionEnd(IInteractable interactable)
        {
            State = IInteractor.InteractorState.Idle;
        }
    }
}