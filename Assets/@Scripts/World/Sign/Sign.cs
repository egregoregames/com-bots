using ComBots.Game.Interactions;
using ComBots.Game.Players;
using ComBots.Game.StateMachine;
using ComBots.Logs;
using ComBots.UI.OverheadWidgets;
using ComBots.Utils.ObjectPooling;
using UnityEngine;

namespace ComBots.World.Signs
{
    public class Sign : MonoBehaviour, IInteractable
    {

        public Transform T => transform;
        public bool IsActive => true;

        [Header("Sign Text")]
        [TextArea(3, 10)]
        [SerializeField] private string _signText;

        [Header("Interact Widget")]
        [SerializeField] private Vector3 _interactWidgetOffset;
        private const string PK_INTERACT_WIDGET = "Sign_Interact_Widget";
        private OverheadWidget _interactWidget;

        private IInteractor _currentInteractor;
        
        #region Unity Lifecycle
        // ----------------------------------------
        // Unity Lifecycle 
        // ----------------------------------------

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + _interactWidgetOffset, 0.1f);
        }

        #endregion

        #region IInteractable Interface

        public bool CanInteract(IInteractor interactor)
        {
            if (interactor is not Player)
            {
                return false;
            }
            return true;
        }

        public void OnInteractionStart(IInteractor interactor)
        {
            if (_interactWidget)
            {
                PoolManager.I.Push(PK_INTERACT_WIDGET, _interactWidget);
                _interactWidget = null;
            }
            _currentInteractor = interactor;
            State_Sign_Args stateArgs = new (_signText, StateSign_OnExit);
            GameStateMachine.I.SetState<GameStateMachine.State_Sign>(stateArgs);
        }

        public void OnInteractorFar(IInteractor interactor)
        {
            if (_interactWidget)
            {
                PoolManager.I.Push(PK_INTERACT_WIDGET, _interactWidget);
                _interactWidget = null;
            }
        }

        public void OnInteractorNearby(IInteractor interactor)
        {
            if (!IsActive)
            {
                return;
            }

            if (!_interactWidget)
            {
                _interactWidget = PoolManager.I.Pull<OverheadWidget>(PK_INTERACT_WIDGET);
                _interactWidget.transform.position = transform.position + _interactWidgetOffset;
            }
        }

        public void OnInteractionEnd(IInteractor interactor)
        {
        }

        #endregion

        #region GameStateMachine API
        
        private void StateSign_OnExit()
        {
            InteractionManager.I.EndInteraction(_currentInteractor, this);
        }
        
        #endregion
    }
}