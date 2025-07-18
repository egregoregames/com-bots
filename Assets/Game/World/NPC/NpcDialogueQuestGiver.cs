using PixelCrushers.QuestMachine.Wrappers;
using UnityEngine;

namespace Game.World.NPC
{
    /// <summary>
    /// Starts a dialogue conversation using Dialogue System and the quest system to assign a quest.
    /// </summary>
    [RequireComponent(typeof(QuestGiver))]
    public class NpcDialogueQuestGiver: MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] NpcSo npcSo;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        QuestGiver _questGiver;
        DialoguePromptCoordinator _dialoguePromptCoordinator;
        
        void Awake()
        {
            _dialoguePromptCoordinator = new DialoguePromptCoordinator(talkPrompt, uiSo);
            _questGiver = GetComponent<QuestGiver>();
            SetQuestsToGive();
        }

        public void Interact(GameObject interactor)
        {
            _dialoguePromptCoordinator.InitializeForInteractor(interactor);
            DialogueUtils.OverrideNpcDialogueDisplayData(npcSo);
            
            _questGiver.StartDialogue(interactor);
            Debug.Log("Quest conversation started name: " + transform.name);
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
        
        void SetQuestsToGive()
        {
            foreach (var quest in npcSo.questsToGive)
            {
                _questGiver.AddQuest(quest);
            }
        }
    }
}
