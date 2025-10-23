using UnityEngine;
using ComBots.Game.Interactions;
using ComBots.Game.StateMachine;
using PixelCrushers.DialogueSystem;
using ComBots.Game.Players;
using ComBots.Logs;
using ComBots.Utils.ObjectPooling;
using ComBots.UI.OverheadWidgets;
using ComBots.Cameras;
using System.Collections;

namespace ComBots.NPCs
{
    public class NPC : MonoBehaviour, IInteractable
    {
        public Transform T => transform;
        [SerializeField] private DialogueActor _dialogueActor;
        [SerializeField] private string _conversationTitle;
        private IInteractor _currentInteractor;

        [Header("Animations")]
        [SerializeField] private Animator _animator;

        [Header("Overhead Widget")]
        [SerializeField] private Vector3 _overheadWidgetOffset;

        [Header("Cameras")]
        public CameraTarget CameraTarget;
        /// <summary> The NPC's rotation at the start of the game </summary>
        private Quaternion _initialRot;
        /// <summary> Pool key for the overhead widget </summary>
        private const string PK_OVERHEAD_WIDGET = "NPC_Overhead_Widget";
        private OverheadWidget _overheadWidget;
        private Coroutine _returnToIdleCoroutine;

        void Awake()
        {
            _initialRot = transform.rotation;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + _overheadWidgetOffset, 0.1f);
        }

        #region IInteractable Implementation
        public void OnInteractionStart(IInteractor interactor)
        {
            if (interactor is not Player player)
            {
                MyLogger<NPC>.StaticLogError($"Only players can interact with NPCs. {interactor} is not a player.");
                return;
            }
            if (_returnToIdleCoroutine != null)
            {
                StopCoroutine(_returnToIdleCoroutine);
            }
            // Remove overhead widget
            if (_overheadWidget)
            {
                PoolManager.I.Push(PK_OVERHEAD_WIDGET, _overheadWidget);
                _overheadWidget = null;
            }
            // Start dialogue
            _currentInteractor = interactor;
            transform.rotation = Quaternion.LookRotation(interactor.T.position - transform.position);
            transform.eulerAngles = new(0, transform.eulerAngles.y, 0);
            State_Dialogue_PixelCrushers_Args args = new(_conversationTitle, _dialogueActor, player.DialogueActor, CameraTarget, player.PlayActorAnimation, PlayConversantAnimation, StateDialogue_OnEnd);
            GameStateMachine.I.SetState<GameStateMachine.State_Dialogue>(args);
        }

        public void OnInteractorFar(IInteractor interactor)
        {
            if (_overheadWidget)
            {
                PoolManager.I.Push(PK_OVERHEAD_WIDGET, _overheadWidget);
                _overheadWidget = null;
            }
        }

        public void OnInteractorNearby(IInteractor interactor)
        {
            if (_overheadWidget == null)
            {
                _overheadWidget = PoolManager.I.Pull<OverheadWidget>(PK_OVERHEAD_WIDGET);
                _overheadWidget.transform.position = transform.position + _overheadWidgetOffset;
            }
        }

        public void OnInteractionEnd(IInteractor interactor)
        {
            if (_returnToIdleCoroutine != null)
            {
                StopCoroutine(_returnToIdleCoroutine);
            }
            _returnToIdleCoroutine = StartCoroutine(Async_ReturnToIdle());
        }

        #endregion

        private void PlayConversantAnimation(string animation)
        {
            MyLogger<NPC>.StaticLog($"NPC.PlayConversantAnimation({animation})");
            if (animation == "Talk")
            {
                _animator.SetBool(animation, true);
            }
            else
            {
                _animator.SetBool("Talk", false);
                if (animation != string.Empty)
                {
                    _animator.SetTrigger(animation);
                }
            }
        }

        private IEnumerator Async_ReturnToIdle()
        {
            yield return new WaitForSeconds(3f);
            // Smoothly rotate back to idle rotation
            float elapsedTime = 0f;
            float duration = 0.7f;
            Quaternion startingRotation = transform.rotation;
            while (elapsedTime < duration)
            {
                transform.rotation = Quaternion.Slerp(startingRotation, _initialRot, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        private void StateDialogue_OnEnd()
        {
            InteractionManager.I.EndInteraction(_currentInteractor, this);
            _animator.SetBool("Talk", false);
            _currentInteractor = null;
        }
    }
}