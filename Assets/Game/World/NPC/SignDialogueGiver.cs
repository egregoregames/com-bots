using Game.World.NPC;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Attach this to a sign and modify the sign text and on interaction with the player it'll show a simple
    /// Dialogue text with its custom UI if there's any attached.
    /// </summary>
    [RequireComponent(typeof(DialogueSystemEvents))]
    public class SignDialogueGiver : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject talkPrompt;
        [SerializeField] string signText;
        [SerializeField] GameEventRelay gameEventRelay;
        [SerializeField] UISo uiSo;
        DialogueSystemEvents _dialogueSystemEvents;
        DialoguePromptCoordinator _dialoguePromptCoordinator;
        const string SIGN_CONVERSATION_KEY = "SIGN";


        public void Awake()
        {
            _dialoguePromptCoordinator = new DialoguePromptCoordinator(talkPrompt,uiSo);
            _dialogueSystemEvents = GetComponent<DialogueSystemEvents>();
        }

        public void Interact(GameObject interactor)
        {
            _dialoguePromptCoordinator.InitializeForInteractor(interactor);
            _dialogueSystemEvents.conversationEvents.onConversationLine.RemoveAllListeners();
            _dialogueSystemEvents.conversationEvents.onConversationLine.AddListener(SetupSignText);
            
            DialogueManager.StartConversation(SIGN_CONVERSATION_KEY, interactor.transform, transform);
            Debug.Log("SIGN attempted conversation started name: " + transform.name);
            
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
        
        void SetupSignText(Subtitle subtitle)
        {
            if (subtitle.dialogueEntry.id == 0) return; // Ignore <START> node.
            subtitle.formattedText.text = signText;
        }
    }
}
