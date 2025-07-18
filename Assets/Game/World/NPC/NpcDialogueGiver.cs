using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game.World.NPC
{
    /// <summary>
    /// Starts a dialogue conversation using Dialogue System.
    /// </summary>
    public class NpcDialogueGiver : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        DialoguePromptCoordinator _dialoguePromptCoordinator;

        void Awake()
        {
            _dialoguePromptCoordinator = new DialoguePromptCoordinator(talkPrompt,uiSo);
        }
        
        public void Interact(GameObject interactor)
        {
            _dialoguePromptCoordinator.InitializeForInteractor(interactor);
            DialogueUtils.OverrideNpcDialogueDisplayData(npcSo);
            DialogueManager.StartConversation(npcSo.conversationKey, interactor.transform, transform);
            Debug.Log("NPC conversation started name: " + transform.name);
        }
        
        public void OnHoverStay()
        {
            _dialoguePromptCoordinator.PromptTalkBubble();
        }

        public void OnHoverEnter()
        {
        }

        public void OnHoverExit()
        {
            _dialoguePromptCoordinator.DeactivateTalkBubble();
        }
    }
}
