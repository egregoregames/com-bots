using System;
using PixelCrushers.DialogueSystem;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class SimpleNpc : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        [SerializeField] bool stopPlayerOnDialogue = true;
        GameObject _interactor;
        
        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            ClearPlayerControllerStoppingCallbacks();
            SetPlayerControllerStoppingCallbacks();
            
            DialogueManager.StartConversation(npcSo.conversationKey, interactor.transform, transform);
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
        }
        
        void ClearPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted -= (StopPlayerOnThisNPCDialogue);
            DialogueManager.instance.conversationEnded -= (ResumePlayerOnThisNPCDialogue);
        }
        
        void StopPlayerOnThisNPCDialogue(Transform actor)
        {
            talkPrompt.SetActive(false);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null && stopPlayerOnDialogue)
            {
                controller.DisAllowPlayerInput();
            }
        }
        
        void ResumePlayerOnThisNPCDialogue(Transform actor)
        {
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null && stopPlayerOnDialogue)
            {
                controller.AllowPlayerInput();
            }
        }
    }
}
