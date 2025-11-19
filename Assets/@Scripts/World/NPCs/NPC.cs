using UnityEngine;
using ComBots.Game.Interactions;
using ComBots.Game.StateMachine;
using PixelCrushers.DialogueSystem;
using ComBots.Game.Players;
using ComBots.Logs;
using ComBots.UI.OverheadWidgets;
using ComBots.Cameras;
using System.Collections;
using ComBots.Utils;
using ComBots.TimesOfDay;

namespace ComBots.World.NPCs
{
    public class NPC : MonoBehaviour, IInteractable
    {
        // ============ IInteractable Implementation ============ //
        public Transform T => transform;
        public bool IsActive => _isActive;
        // =============== Active State Config =============== //
        [Header("Active State Config")]
        public NPC_ActiveStateConfig ActiveStateConfig;
        // =============== PixelCrushers Dialogue =============== //
        [Header("PixelCrushers Dialogue")]
        [SerializeField] private DialogueActor _dialogueActor;
        [Tooltip("Conversation name in PixelCrushers Dialogue DataBase")]
        public string _conversationTitle;

        [Header("Visuals")]
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _visual;

        [Header("Overhead Widget")]
        [SerializeField] private Vector3 _overheadWidgetOffset;
        private WC_OverheadWidget _overheadWidget;

        [Header("Cameras")]
        public CameraTarget CameraTarget;

        // ============ State ============ //
        /// <summary> Is affected by visibility config in NPC_Config. </summary>
        private bool _isActive;

        // ============ Cache ============ //
        /// <summary> The NPC's rotation at the start of the game </summary>
        private Quaternion _initialRot;
        private Coroutine _returnToIdleCoroutine;
        private IInteractor _currentInteractor;

        #region Unity Methods
        // ----------------------------------------
        // Unity Methods 
        // ----------------------------------------

        void OnEnable()
        {
            TimesOfDay_Manager.Async_SubscribeToTimeOfDayChange(TimesOfDay_Manager_OnTimeOfDayChanged);
        }

        void OnDisable()
        {
            TimesOfDay_Manager.Async_UnsubscribeFromTimeOfDayChange(TimesOfDay_Manager_OnTimeOfDayChanged);
        }

        void Awake()
        {
            _initialRot = transform.rotation;
            if (!_visual)
                _visual = transform.GetChild(0).gameObject;
            if (_visual)
                _isActive = _visual.activeSelf;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position + _overheadWidgetOffset, 0.1f);
        }
        #endregion

        #region IInteractable Implementation
        // ----------------------------------------
        // IInteractable Implementation 
        // ----------------------------------------

        public bool CanInteract(IInteractor interactor)
        {
            // Check if interactor is a player
            if (interactor is not Player player)
            {
                return false;
            }
            if (!_isActive)
            {
                return false;
            }
            return true;
        }

        public async void OnInteractionStart(IInteractor interactor)
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
                OverheadWidgetManager.ReturnWidget(_overheadWidget);
                _overheadWidget = null;
            }
            // Rotate NPC towards player
            _currentInteractor = interactor;
            transform.rotation = Quaternion.LookRotation(interactor.T.position - transform.position);
            transform.eulerAngles = new(0, transform.eulerAngles.y, 0);
            // Determine the right conversation
            State_Dialogue_PixelCrushers_Args args = new(_conversationTitle, _dialogueActor, player.DialogueActor, CameraTarget, player.PlayActorAnimation, PlayConversantAnimation, StateDialogue_OnEnd);
            GameStateMachine.I.SetState<GameStateMachine.State_Dialogue>(args);
        }

        public async void OnInteractorFar(IInteractor interactor)
        {
            if (!_isActive) // NPC is not active
            {
                return;
            }

            if (_overheadWidget)
            {
                OverheadWidgetManager.ReturnWidget(_overheadWidget);
                _overheadWidget = null;
            }
        }

        public async void OnInteractorNearby(IInteractor interactor)
        {
            if (!_isActive) // NPC is not active
            {
                return;
            }

            if (_overheadWidget == null)
            {
                _overheadWidget = await OverheadWidgetManager.GetWidget(OverheadWidgetType.Talk);
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

        #region State Management API
        // ----------------------------------------
        // State Management API 
        // ----------------------------------------

        private void StateDialogue_OnEnd()
        {
            InteractionManager.I.EndInteraction(_currentInteractor, this);
            _animator.SetBool("Talk", false);
            _currentInteractor = null;
        }

        #endregion

        #region TimesOfDay Manager
        // ----------------------------------------
        // TimesOfDay Manager
        // ----------------------------------------

        private void TimesOfDay_Manager_OnTimeOfDayChanged(TimeOfDay newTimeOfDay)
        {
            UpdateActiveStatus(PersistentGameData.Instance.CurrentTerm, newTimeOfDay);
        }

        #endregion

        #region Private Methods
        // ----------------------------------------
        // Private Methods
        // ----------------------------------------

        private void UpdateActiveStatus(Term term, TimeOfDay timeOfDay)
        {
            //Debug.Log($"NPC.UpdateActiveStatus({term}, {timeOfDay})");
            // Check currentterm in visibility config.terms
            bool isTimeConditionSatisfied = ActiveStateConfig.TimeCondition.IsStatisfied(term, timeOfDay);
            SetActive(isTimeConditionSatisfied);
        }

        private void SetActive(bool active)
        {
            if (active == _isActive)
            {
                return;
            }
            MyLogger<NPC>.StaticLog($"SetActive({active})");

            _isActive = active;
            // Show/hide visual
            if (_visual)
                _visual.SetActive(active);

            // Hide overhead widget if inactive
            if (!_isActive && _overheadWidget)
            {
                OverheadWidgetManager.ReturnWidget(_overheadWidget);
                _overheadWidget = null;
            }
        }

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

        #endregion
    }
}