using PixelCrushers.DialogueSystem;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    /// <summary>
    /// Attach this to a sign and modify the sign text and on interaction with the player it'll show a simple
    /// Dialogue text with its custom UI if there's any attached.
    /// </summary>
    [RequireComponent(typeof(DialogueSystemEvents))]
    public class SignDialoguePresenter : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] string signText;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        DialogueSystemEvents _dialogueSystemEvents;
        GameObject _interactor;
        const string SIGN_CONVERSATION_KEY = "SIGN";


        public void Awake()
        {
            _dialogueSystemEvents = GetComponent<DialogueSystemEvents>();
        }

        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            SetPlayerControllerStoppingCallbacks();
            DialogueManager.StartConversation(SIGN_CONVERSATION_KEY, interactor.transform, transform);
            Debug.Log("SIGN attempted conversation started name: " + transform.name);
        }
        
        public void OnHoverStay()
        {
            if (!DialogueManager.isConversationActive)
            {
                talkPrompt.SetActive(true);
            }
        }

        public void OnHoverEnter()
        {
        }

        public void OnHoverExit()
        {
            talkPrompt.SetActive(false);
        }
        
        [ContextMenu("Go")]
        public void GoToPlayer()
        {
            GetComponent<NavMeshAgent>().SetDestination(_interactor.transform.position);
        }
        
        void SetPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted += (StopPlayerOnThisNPCDialogue);
            DialogueManager.instance.conversationEnded += (ResumePlayerOnThisNPCDialogue);
            _dialogueSystemEvents.conversationEvents.onConversationLine.AddListener(SetupSignText);
        }
        
        void ClearPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted -= (StopPlayerOnThisNPCDialogue);
            DialogueManager.instance.conversationEnded -= (ResumePlayerOnThisNPCDialogue);
            _dialogueSystemEvents.conversationEvents.onConversationLine.RemoveListener(SetupSignText);
        }
        
        void StopPlayerOnThisNPCDialogue(Transform actor)
        {
            uiSo.OnCameraTransition?.Invoke(false);
            talkPrompt.SetActive(false);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null)
            {
                controller.DisAllowPlayerInput();
            }
        }
        
        void ResumePlayerOnThisNPCDialogue(Transform actor)
        {
            ClearPlayerControllerStoppingCallbacks();
            uiSo.OnCameraTransition?.Invoke(true);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null)
            {
                controller.AllowPlayerInput();
            }
        }
        
        void SetupSignText(Subtitle subtitle)
        {
            if (subtitle.dialogueEntry.id == 0) return; // Ignore <START> node.
            subtitle.formattedText.text = signText;
        }
    }
}
