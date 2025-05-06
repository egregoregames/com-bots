using PixelCrushers.DialogueSystem;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [RequireComponent(typeof(DialogueSystemEvents))]
    public class SimpleNpc : MonoBehaviour, IInteractable
    {
        
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        [SerializeField] bool stopPlayerOnDialogue = true;
        GameObject _player;
        DialogueSystemEvents _dialogueEvents;

        
        void OnValidate()
        {
            if (_dialogueEvents == null) _dialogueEvents = GetComponent<DialogueSystemEvents>();
            
            ClearPlayerControllerStoppingCallbacks();
            
            if (stopPlayerOnDialogue)
            {
                SetPlayerControllerStoppingCallbacks();
            }
        }
        
        public void Interact(GameObject interactor)
        {
            _player = interactor;
            DialogueManager.StartConversation(npcSo.conversationKey, interactor.transform, transform);
            Debug.Log("NPC conversation started name: " + transform.name);
        }
        
        public void OnHoverStay()
        {
            talkPrompt.SetActive(true);
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
            GetComponent<NavMeshAgent>().SetDestination(_player.transform.position);
        }
        
        void SetPlayerControllerStoppingCallbacks()
        {
            _dialogueEvents.conversationEvents.onConversationStart.AddListener(StopPlayerOnThisNPCDialogue);
            _dialogueEvents.conversationEvents.onConversationEnd.AddListener(ResumePlayerOnThisNPCDialogue);
        }
        
        void ClearPlayerControllerStoppingCallbacks()
        {
            _dialogueEvents.conversationEvents.onConversationStart.RemoveAllListeners();
            _dialogueEvents.conversationEvents.onConversationEnd.RemoveAllListeners();
        }
        
        void StopPlayerOnThisNPCDialogue(Transform actor)
        {
            var controller = _player.GetComponentInParent<ThirdPersonController>();

            if (controller != null)
            {
                controller.DisAllowPlayerInput();
            }
        }
        
        void ResumePlayerOnThisNPCDialogue(Transform actor)
        {
            var controller = _player.GetComponentInParent<ThirdPersonController>();

            if (controller != null)
            {
                controller.AllowPlayerInput();
            }
        }
    }
}
