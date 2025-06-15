using System;
using PixelCrushers.DialogueSystem;
using PixelCrushers.QuestMachine.Wrappers;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;

namespace Game.World.NPC
{
    [RequireComponent(typeof(QuestGiver))]
    public class NpcDialogueQuestGiver: MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        [SerializeField] bool stopPlayerOnDialogue = true;
        GameObject _interactor;
        QuestGiver _questGiver;

        void Awake()
        {
            _questGiver = GetComponent<QuestGiver>();
            foreach (var quest in npcSo.questsToGive)
            {
                //_questGiver.AddQuest(quest);
            }
        }

        public void Interact(GameObject interactor)
        {
            _interactor = interactor;
            SetPlayerControllerStoppingCallbacks();
            _questGiver.StartDialogue(_interactor);
            Debug.Log("Quest conversation started name: " + transform.name);
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
            uiSo.OnCameraTransition?.Invoke(false);
            talkPrompt.SetActive(false);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null && stopPlayerOnDialogue)
            {
                controller.DisAllowPlayerInput();
            }
        }
        
        void ResumePlayerOnThisNPCDialogue(Transform actor)
        {
            ClearPlayerControllerStoppingCallbacks();
            uiSo.OnCameraTransition?.Invoke(true);
            var controller = _interactor.GetComponentInParent<ThirdPersonController>();
            if (controller != null && stopPlayerOnDialogue)
            {
                controller.AllowPlayerInput();
            }
        }
    }
}
