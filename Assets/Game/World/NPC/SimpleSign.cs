using System;
using PixelCrushers.DialogueSystem;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(DialogueSystemEvents))]
    public class SimpleSign : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] string signText;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        DialogueSystemEvents _dialogueSystemEvents;
        GameObject _interactor;
        const string SIGN_CONVERSATION_KEY = "New Conversation 3";


        public void Awake()
        {
            _dialogueSystemEvents = GetComponent<DialogueSystemEvents>();
        }

        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            ClearPlayerControllerStoppingCallbacks();
            SetPlayerControllerStoppingCallbacks();
            
            DialogueManager.StartConversation(SIGN_CONVERSATION_KEY, interactor.transform, transform);
            Debug.Log("NPC conversation started name: " + transform.name);
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
            _dialogueSystemEvents.conversationEvents.onConversationLine.AddListener(OnConversationLine);
        }
        
        void ClearPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted -= (StopPlayerOnThisNPCDialogue);
            DialogueManager.instance.conversationEnded -= (ResumePlayerOnThisNPCDialogue);
            _dialogueSystemEvents.conversationEvents.onConversationLine.RemoveListener(OnConversationLine);
        }
        
        void StopPlayerOnThisNPCDialogue(Transform actor)
        {
            talkPrompt.SetActive(false);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null)
            {
                controller.DisAllowPlayerInput();
            }
        }
        
        void ResumePlayerOnThisNPCDialogue(Transform actor)
        {
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null)
            {
                controller.AllowPlayerInput();
            }
        }
        
        void OnConversationLine(Subtitle subtitle)
        {
            if (subtitle.dialogueEntry.id == 0) return; // Ignore <START> node.
            subtitle.formattedText.text = signText;
        }
    }
}
