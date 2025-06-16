using PixelCrushers.DialogueSystem;
using StarterAssets;
using UnityEngine;

namespace Game.World.NPC
{
    /// <summary>
    /// Handles dialogue UI prompts and player input control during conversations. Manages showing talk prompts on hover
    /// And subscribing to dialogue start/end events to disable and enable player input accordingly.
    /// </summary>
    public class DialoguePromptCoordinator
    {
        readonly UISo _uiSo;
        readonly GameObject _talkPrompt;
        GameObject _interactor;
        
        public DialoguePromptCoordinator(GameObject talkPrompt, UISo uiSo)
        {
            _uiSo = uiSo;
            _talkPrompt = talkPrompt;
        }
        
        public void InitializeForInteractor(GameObject interactor)
        {
            _interactor = interactor;
            SetPlayerControllerStoppingCallbacks();
        }

        public void PromptTalkBubble()
        {
            if (!DialogueManager.isConversationActive)
                _talkPrompt?.SetActive(true);
        }
        
        public void DeactivateTalkBubble()
        {
            _talkPrompt?.SetActive(false);
        }
        
        void SetPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted += StopPlayer;
            DialogueManager.instance.conversationEnded += ResumePlayer;
        }

        void ClearPlayerControllerStoppingCallbacks()
        {
            DialogueManager.instance.conversationStarted -= StopPlayer;
            DialogueManager.instance.conversationEnded -= ResumePlayer;
        }

        void StopPlayer(Transform actor)
        {
            _uiSo.OnCameraTransition?.Invoke(false);
            _talkPrompt?.SetActive(false);
            var controller = _interactor?.GetComponentInParent<ThirdPersonController>();
            if (controller != null) controller.DisAllowPlayerInput();
        }

        void ResumePlayer(Transform actor)
        {
            _uiSo.OnCameraTransition?.Invoke(true);
            var controller = _interactor?.GetComponentInParent<ThirdPersonController>();
            if (controller != null) controller.AllowPlayerInput();

            ClearPlayerControllerStoppingCallbacks();
        }
    }
}
