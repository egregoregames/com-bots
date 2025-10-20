using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using ComBots.UI.Utilities.Listing;
using TMPro;
using UnityEngine.UI;

namespace ComBots.Global.UI.Dialogue
{
    public class DialogueController : UIController, IInputHandler, IDialogueUI
    {
        protected override string UserInterfaceName => "Global.Dialogue";

        public override Dependency Dependency => Dependency.Independent;

        private bool _isActive;

        [Header("UI")]
        [SerializeField] private GameObject _w_root;

        [Header("Dialogue")]
        [SerializeField] private TextMeshProUGUI _dialogueText;
        private Tween _textAnimationTween;

        [Header("Utility Lister")]
        [SerializeField] private WC_Lister<WC_DialogueOption> _optionLister;

        [Header("Dialogue Events")]
        [SerializeField] private DialogueSystemEvents _dialogueSystemEvents;

        [Header("NameTag")]
        [SerializeField] private GameObject _nametag;
        [SerializeField] private Image _nametagIcon;
        [SerializeField] private TextMeshProUGUI _nametagText;

        [Header("End & Continue")]
        [SerializeField] private GameObject _dialogContinueIcon;
        [SerializeField] private GameObject _dialogEndIcon;
        // Responses
        private Response[] _pcArgs_responsesBuffer;
        private Coroutine _COR_responses;

        // Args
        private IState_Dialogue_Args _args;

        // IDialogueUI implementation
        public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;

        #region Initialization & Disposal
        // ----------------------------------------
        // Initialization & Disposal
        // ----------------------------------------
        protected override void Init()
        {
            // Register this as the DialogueManager's UI
            DialogueManager.dialogueUI = this;
            // Check Display Settings
            if (DialogueManager.displaySettings != null)
            {
                MyLogger<DialogueController>.StaticLog($"Display Settings found. Checking subtitle settings...");
                MyLogger<DialogueController>.StaticLog($"DialogueManager.displaySettings.subtitleSettings type: {DialogueManager.displaySettings.subtitleSettings?.GetType().Name}");
            }
            else
            {
                MyLogger<DialogueController>.StaticLogWarning("DialogueManager.displaySettings is null!");
            }
        }

        public override void Dispose()
        {
            // Unregister from DialogueManager if we're the active UI
            if (ReferenceEquals(DialogueManager.dialogueUI, this))
            {
                DialogueManager.dialogueUI = null;
            }

            _w_root = null;
            _dialogueText = null;
            _optionLister?.Dispose();
            _optionLister = null;
            _dialogContinueIcon = null;
            _dialogEndIcon = null;
            _args = null;
        }
        #endregion

        #region State Machine API
        // ----------------------------------------
        // Game State Machine API
        // ----------------------------------------

        public void SetActive(IState_Dialogue_Args args)
        {
            if (_isActive)
            {
                SetInactive();
            }
            _isActive = true;
            _args = args;

            // Hide icons at start
            _dialogEndIcon.SetActive(false);
            _dialogContinueIcon.SetActive(false);

            if (args is State_Dialogue_PixelCrushers_Args pixelCrushersArgs)
            {
                MyLogger<DialogueController>.StaticLog($"Starting PixelCrushers conversation '{pixelCrushersArgs.ConversationTitle}' between '{pixelCrushersArgs.Actor.GetActorName()}' and '{pixelCrushersArgs.Conversant.GetActorName()}'");
                MyLogger<DialogueController>.StaticLog($"Current DialogueManager.dialogueUI: {DialogueManager.dialogueUI?.GetType().Name}");
                MyLogger<DialogueController>.StaticLog($"Is this the registered UI? {ReferenceEquals(DialogueManager.dialogueUI, this)}");
                DialogueManager.StartConversation(pixelCrushersArgs.ConversationTitle, pixelCrushersArgs.Actor.transform, pixelCrushersArgs.Conversant.transform);
                // DisplayGUI() Will be called automatically by DialogueManager using Open()
            }
            else if (args is State_Dialogue_Args standardArgs)
            {
                ShowSubtitle(standardArgs.Dialogue, 2f, true);
                DisplayGUI();
            }
        }

        public void SetInactive()
        {
            if (!_isActive) { return; }
            _isActive = false;

            // Stop the Pixel Crushers Dialogue
            if (DialogueManager.isConversationActive)
            {
                MyLogger<DialogueController>.StaticLog($"PixelCrushers.DialogueManager.StopConversation()");
                DialogueManager.StopConversation();
            }
            // Kill the text animation if it's still running
            if (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                _textAnimationTween.Kill();
                _textAnimationTween = null;
            }
            // Hide Dialogue UI
            _dialogContinueIcon.SetActive(false);
            _dialogEndIcon.SetActive(false);

            HideResponses();
            _w_root.SetActive(false);
            // Clear args
            _args = null;
            MyLogger<DialogueController>.StaticLog($"SetInactive()");
        }

        /// <summary>
        /// This must be called by the state machine when the dialogue state is exited.
        /// </summary>
        public void OnExit()
        {
            // Clear args after handling
            _args = null;
        }

        #endregion

        #region InputManager API
        // ----------------------------------------
        // Input 
        // ----------------------------------------

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            // Ignore input while text is animating
            if (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                return true; // Consume the input but don't process it
            }

            // Handle non-lister inputs (like proceeding through dialogue)
            switch (actionName)
            {
                case DialogueInputHandler.INPUT_ACTION_CONFIRM:
                    if (!context.performed) { return true; }
                    if (_optionLister.IsActive) // If we have options pass the input
                    {
                        _optionLister.Input_Confirm();
                    }
                    else if (_args is State_Dialogue_PixelCrushers_Args) // Move-on to next node
                    {
                        MyLogger<DialogueController>.StaticLog("Advancing PixelCrushers conversation.");
                        DialogueManager.instance.SendMessage(DialogueSystemMessages.OnConversationContinue, (IDialogueUI)this, SendMessageOptions.DontRequireReceiver);
                    }
                    else // Go back to playing state
                    {
                        GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
                    }
                    return true;
                case DialogueInputHandler.INPUT_ACTION_CANCEL:
                    if (!context.performed) { return true; }
                    //GameStateMachine.I.ExitState<GameStateMachine.State_Dialogue>();
                    // return true;
                    return false;
                case DialogueInputHandler.INPUT_ACTION_NAVIGATE:
                    if (!context.performed) { return true; }
                    if (!_optionLister.IsActive) { break; }
                    Vector2 inputValue = context.ReadValue<Vector2>();
                    _optionLister.Input_Navigate(new(inputValue.x, -inputValue.y));
                    return true;
            }

            return false;
        }

        public void OnInputContextEntered(InputContext context)
        {
        }

        public void OnInputContextExited(InputContext context)
        {
        }

        public void OnInputContextPaused(InputContext context)
        {
        }

        public void OnInputContextResumed(InputContext context)
        {
        }
        #endregion

        #region Pixel Crushers API
        // ----------------------------------------
        // Pixel Crushers IDialogueUI Implementation
        // ----------------------------------------
        public void Open()
        {
            DisplayGUI();
        }

        public void Close()
        {
            if (_isActive)
            {
                GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
            }
        }

        public void ShowSubtitle(Subtitle subtitle)
        {
            MyLogger<DialogueController>.StaticLog($"ShowSubtitle called - Title: '{subtitle.dialogueEntry.Title}', Actor: '{subtitle.speakerInfo.nameInDatabase}', Text: '{subtitle.formattedText.text}', Sequence: '{subtitle.sequence}'");

            // If it's empty text (like the START node), continue immediately
            if (string.IsNullOrEmpty(subtitle.formattedText.text))
            {
                MyLogger<DialogueController>.StaticLog($"Empty subtitle, continuing...");
                StartCoroutine(ContinueAfterFrame());
                return;
            }

            // Only skip if this is actually a player response that we want to skip
            if (subtitle.speakerInfo.nameInDatabase == "Player")
            {
                MyLogger<DialogueController>.StaticLog($"Skipping player response: {subtitle.dialogueEntry.DialogueText}");
                StartCoroutine(ContinueAfterFrame());
                return;
            }

            // For NPC dialogue, show it
            MyLogger<DialogueController>.StaticLog($"Displaying NPC dialogue from {subtitle.speakerInfo.nameInDatabase}: {subtitle.formattedText.text}");

            // Set the speaker name
            if (subtitle.speakerInfo != null && !string.IsNullOrEmpty(subtitle.speakerInfo.nameInDatabase))
            {
                _nametagText.text = subtitle.speakerInfo.nameInDatabase;
                _nametag.SetActive(true);
            }
            else
            {
                _nametag.SetActive(false);
            }

            // Check if this subtitle has a very short or "None()" sequence that would cause immediate hiding
            if (string.IsNullOrEmpty(subtitle.sequence) || subtitle.sequence.Contains("None()@0"))
            {
                MyLogger<DialogueController>.StaticLog($"Subtitle has short/empty sequence '{subtitle.sequence}', using custom timing");
                _dialogueText.text = subtitle.formattedText.text;
            }
            else
            {
                // float charsPerSecond = DialogueManager.displaySettings.subtitleSettings.subtitleCharsPerSecond;
                // float minSeconds = DialogueManager.displaySettings.subtitleSettings.minSubtitleSeconds;
                //float animationDuration = Mathf.Max(minSeconds, subtitle.formattedText.text.Length / charsPerSecond);
                float animationDuration = 0.02f * subtitle.formattedText.text.Length; // 20 chars per second
                AnalyzeSubtitle(subtitle, out bool isLastSubtitle, out bool _);
                ShowSubtitle(subtitle.formattedText.text, animationDuration, isLastSubtitle);
            }

            // Play conversant animation
            if (_args is State_Dialogue_PixelCrushers_Args pcArgs && pcArgs.PlayConversantAnimation != null)
            {
                string value = string.Empty;
                for (int i = 0; i < subtitle.dialogueEntry.fields.Count; i++)
                {
                    if (subtitle.dialogueEntry.fields[i].title == "ConversantAnimation")
                    {
                        value = subtitle.dialogueEntry.fields[i].value;
                        break;
                    }
                }
                if (value != string.Empty)
                {
                    pcArgs.PlayConversantAnimation?.Invoke(value);
                }
                else
                {
                    pcArgs.PlayConversantAnimation?.Invoke("Talk");
                }
            }
        }
        #endregion

        private void AnalyzeSubtitle(Subtitle subtitle, out bool isLast, out bool hasOptions)
        {
            try
            {
                var conversationModel = DialogueManager.conversationModel;

                if (conversationModel == null || subtitle?.dialogueEntry == null)
                {
                    isLast = true;
                    hasOptions = false;
                    return;
                }

                // Get the state for this dialogue entry, including evaluating its links
                var currentState = conversationModel.GetState(subtitle.dialogueEntry, includeLinks: true);

                if (currentState == null)
                {
                    isLast = true;
                    hasOptions = false;
                    return;
                }

                // Check if there are any player responses available
                hasOptions = currentState.hasAnyResponses;

                // If no responses, this is the last subtitle
                isLast = !hasOptions;
            }
            catch (System.Exception ex)
            {
                MyLogger<DialogueController>.StaticLogError($"Error analyzing subtitle: {ex.Message}");
                isLast = true;
                hasOptions = false;
            }
        }

        private IEnumerator ContinueAfterFrame()
        {
            yield return null; // Wait one frame
            DialogueManager.instance.SendMessage(DialogueSystemMessages.OnConversationContinue,
                                               (IDialogueUI)this,
                                               SendMessageOptions.DontRequireReceiver);
        }

        private void ShowSubtitle(string text, float animationDuration, bool isLast)
        {
            MyLogger<DialogueController>.StaticLog($"Showing subtitle: {text}");

            // Hide end & continue icons
            _dialogEndIcon.SetActive(false);
            _dialogContinueIcon.SetActive(false);
            // Hide responses if any
            HideResponses();

            // Don't show empty text
            if (string.IsNullOrEmpty(text))
            {
                MyLogger<DialogueController>.StaticLog($"Empty text provided to ShowSubtitle, skipping animation");
                return;
            }

            // Display dialogue
            _dialogueText.text = string.Empty;
            _textAnimationTween = DOTween.To(() => _dialogueText.text, x => _dialogueText.text = x, text, animationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _textAnimationTween = null;

                    // Display options if we have them
                    if (_args is State_Dialogue_Args standardArgs && standardArgs.OptionsArgs != null)
                    {
                        StandardArgs_ShowResponses(standardArgs.OptionsArgs.Options);
                    }
                    else if (_args is State_Dialogue_PixelCrushers_Args pcArgs)
                    {
                        pcArgs.PlayConversantAnimation?.Invoke(string.Empty);
                        // If this is the last subtitle, show the end icon
                        if (isLast)
                        {
                            MyLogger<DialogueController>.StaticLog($"Showing end icon for last subtitle.");
                            _dialogEndIcon.SetActive(true);
                            _dialogContinueIcon.SetActive(false);
                        }
                        else
                        {
                            MyLogger<DialogueController>.StaticLog($"Showing continue icon for non-last subtitle.");
                            _dialogEndIcon.SetActive(false);
                            _dialogContinueIcon.SetActive(_COR_responses == null);
                        }
                    }
                });
        }

        public void HideSubtitle(Subtitle subtitle)
        {
            MyLogger<DialogueController>.StaticLog($"HideSubtitle called for: '{subtitle.formattedText.text}' from speaker: '{subtitle.speakerInfo.nameInDatabase}'");

            // Only hide if this is not an NPC subtitle that should stay visible
            if (subtitle.speakerInfo.nameInDatabase != "Player" && !string.IsNullOrEmpty(subtitle.formattedText.text))
            {
                MyLogger<DialogueController>.StaticLog($"Keeping NPC subtitle visible for responses");
                // Don't clear NPC subtitles immediately - they should stay visible during response selection
                return;
            }

            // Clear the dialogue text for empty subtitles or player text
            _dialogueText.text = string.Empty;
            _nametag.SetActive(false);
        }

        public void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            if (_COR_responses != null)
            {
                StopCoroutine(_COR_responses);
            }
            _COR_responses = StartCoroutine(Async_PCArgs_ShowResponses(responses));
        }

        public void HideResponses()
        {
            MyLogger<DialogueController>.StaticLog("Hiding responses.");
            _optionLister.SetInactive();
            _COR_responses = null;
        }

        public void ShowQTEIndicator(int index)
        {
            MyLogger<DialogueController>.StaticLogWarning($"DialogueController.ShowQTEIndicator() is not implemented for index {index}.");
        }

        public void HideQTEIndicator(int index)
        {
            MyLogger<DialogueController>.StaticLogWarning($"DialogueController.HideQTEIndicator() is not implemented for index {index}.");
        }

        public void ShowAlert(string message, float duration)
        {
            MyLogger<DialogueController>.StaticLogWarning("DialogueController.ShowAlert() is not implemented.");
        }

        public void HideAlert()
        {
            MyLogger<DialogueController>.StaticLogWarning("DialogueController.HideAlert() is not implemented.");
        }

        #region Lister API
        // ----------------------------------------
        // Lister API 
        // ----------------------------------------

        private void OptionLister_SetupOption(WC_DialogueOption option, int index)
        {
            if (_args is State_Dialogue_Args standardArgs)
            {
                bool isBackOption = index >= standardArgs.OptionsArgs.Options.Length;
                option.Setup(isBackOption ? standardArgs.OptionsArgs.CancelOption : standardArgs.OptionsArgs.Options[index]);
                return;
            }
            else if (_args is State_Dialogue_PixelCrushers_Args pcArgs)
            {
                bool isBackOption = index >= _pcArgs_responsesBuffer.Length;
                option.Setup(isBackOption ? "Back" : _pcArgs_responsesBuffer[index].formattedText.text);
            }
        }

        private void OptionLister_OnSelected(int index)
        {
            bool isBackOption;
            MyLogger<DialogueController>.StaticLog($"Confirming selection: {index}");
            if (_args is State_Dialogue_Args standardArgs)
            {
                isBackOption = index >= standardArgs.OptionsArgs.Options.Length;
                var callback = standardArgs.OptionsArgs.Callback;
                _args = null; // Clear args before invoking callback
                callback?.Invoke(index);
            }
            else if (_args is State_Dialogue_PixelCrushers_Args pcArgs)
            {
                // Determine if this is the back option
                isBackOption = index >= _pcArgs_responsesBuffer.Length;

                // Figure out the actor animation for this response if any
                string animation = string.Empty;
                if (!isBackOption)
                {
                    for (int i = 0; i < _pcArgs_responsesBuffer[index].destinationEntry.fields.Count; i++)
                    {
                        if (_pcArgs_responsesBuffer[index].destinationEntry.fields[i].title == "ActorAnimation")
                        {
                            animation = _pcArgs_responsesBuffer[index].destinationEntry.fields[i].value;
                            break;
                        }
                    }
                    if (animation != string.Empty)
                    {
                        pcArgs.PlayActorAnimation?.Invoke(animation);
                    }
                    SelectedResponseHandler?.Invoke(this, new SelectedResponseEventArgs(_pcArgs_responsesBuffer[index]));
                }
                else
                {
                    DialogueManager.StopConversation();
                }
            }
        }

        #endregion

        private void StandardArgs_ShowResponses(string[] responses)
        {
            MyLogger<DialogueController>.StaticLog($"Showing responses: {string.Join(", ", responses)}");
            _optionLister.SetActive(responses.Length, OptionLister_SetupOption, null, true);
            _optionLister.SetActive(
                responses.Length,
                OptionLister_SetupOption,
                onOptionSelected: OptionLister_OnSelected,
                true
            );
        }

        private IEnumerator Async_PCArgs_ShowResponses(Response[] responses)
        {
            while (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                yield return null;
            }

            MyLogger<DialogueController>.StaticLog($"Showing {responses.Length} responses...");

            if (_pcArgs_responsesBuffer == null || _pcArgs_responsesBuffer.Length != responses.Length)
            {
                _pcArgs_responsesBuffer = new Response[responses.Length];
            }
            for (int i = 0; i < responses.Length; i++)
            {
                _pcArgs_responsesBuffer[i] = responses[i];
            }
            MyLogger<DialogueController>.StaticLog($"Stored {responses.Length} responses in the buffer.");
            _optionLister.SetActive(
                responses.Length,
                OptionLister_SetupOption,
                OptionLister_OnSelected,
                true
            );
            _COR_responses = null;
        }

        #region GUI Utilities
        // ----------------------------------------
        // GUI Utilities 
        // ----------------------------------------

        private void DisplayGUI()
        {
            if (!_isActive)
            {
                MyLogger<DialogueController>.StaticLogError("Open() was called but the Dialogue Controller isn't active.");
                return;
            }

            // Show UI
            _w_root.SetActive(true);

            // Ensure icons are properly hidden when opening dialogue
            _dialogEndIcon.SetActive(false);
            _dialogContinueIcon.SetActive(false);

            // Handle nametag
            if (string.IsNullOrEmpty(_args.Nametag))
            {
                _nametag.SetActive(false);
            }
            else
            {
                _nametag.SetActive(true);
                _nametagIcon.enabled = false;
                _nametagText.text = _args.Nametag;
            }
        }

        #endregion
    }
}