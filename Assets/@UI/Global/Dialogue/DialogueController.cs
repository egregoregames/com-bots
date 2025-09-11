using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.UI.Utilities;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;

namespace ComBots.Global.UI.Dialogue
{
    public class DialogueController : UIController, IInputHandler, IDialogueUI
    {
        protected override string UserInterfaceName => "Global.Dialogue";

        public override Dependency Dependency => Dependency.Independent;

        private const string CLASS_INACTIVE = "inactive";
        private const string CLASS_DIALOG_LABEL = "dialog-label";
        private const string CLASS_NAMETAG = "nametag";
        private const string CLASS_NAMETAG_LABEL = "nametag-label";
        private const string CLASS_OPTION_LISTER_PARENT = "option-lister-parent";
        private const string CLASS_OPTION_LISTER_PARENT_HIDDEN = "option-lister-parent-hidden";
        private const string CLASS_DIALOG_CONTINUE_ICON = "dialog-continue-icon";
        private const string CLASS_DIALOG_CONTINUE_ICON_HIDDEN = "dialog-continue-icon-hidden";
        private const string CLASS_DIALOG_END_ICON = "dialog-end-icon";
        private const string CLASS_DIALOG_END_ICON_HIDDEN = "dialog-end-icon-hidden";

        private bool _isActive;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        private VisualElement _VE_root;

        [Header("Utility Lister")]
        [SerializeField] private WC_Lister _WC_optionLister;

        [Header("Dialogue Events")]
        [SerializeField] private DialogueSystemEvents _dialogueSystemEvents;

        // Dialog
        private Label _VE_dialogLabel;
        private VisualElement _VE_optionListerParent;
        private VisualElement _VE_nametag;
        private VisualElement _VE_dialogContinueIcon;
        private VisualElement _VE_dialogEndIcon;
        private Label _VE_nametagLabel;
        private Coroutine _COR_responses;

        // Animation state
        private Tween _textAnimationTween;

        // Events
        private IState_Dialogue_Args _args;

        // IDialogueUI implementation
        public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;

        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement.Q(name: "Root");
            _VE_dialogLabel = _VE_root.Q<Label>(className: CLASS_DIALOG_LABEL);
            _VE_nametag = _VE_root.Q(className: CLASS_NAMETAG);
            _VE_nametagLabel = _VE_root.Q<Label>(className: CLASS_NAMETAG_LABEL);
            _VE_optionListerParent = _VE_root.Q(className: CLASS_OPTION_LISTER_PARENT);
            _VE_dialogContinueIcon = _VE_root.Q(className: CLASS_DIALOG_CONTINUE_ICON);
            _VE_dialogEndIcon = _VE_root.Q(className: CLASS_DIALOG_END_ICON);
            _WC_optionLister.TryInit();

            // Register this as the DialogueManager's UI
            MyLogger<DialogueController>.StaticLog($"Registering DialogueController as DialogueManager.dialogueUI");
            DialogueManager.dialogueUI = this;
            MyLogger<DialogueController>.StaticLog($"DialogueManager.dialogueUI is now: {DialogueManager.dialogueUI?.GetType().Name}");

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

            // Optional: Subscribe to some dialogue events for logging/debugging
            // The main integration happens through IDialogueUI interface methods
            // var events = _dialogueSystemEvents.conversationEvents;
            // events.onConversationStart.AddListener(OnConversationStart);
            // events.onConversationEnd.AddListener(OnConversationEnd);
        }

        public override void Dispose()
        {
            // Unregister from DialogueManager if we're the active UI
            if (ReferenceEquals(DialogueManager.dialogueUI, this))
            {
                DialogueManager.dialogueUI = null;
            }

            _VE_root = null;
            _args = null;
        }

        /// <summary>
        /// This must be called by the state machine when the dialogue state is exited.
        /// </summary>
        public void OnExit()
        {
            // Clear args after handling
            _args = null;
        }

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
                    if (_WC_optionLister.IsActive) // If we have options pass the input
                    {
                        _WC_optionLister.Input_Confirm();
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
                    GameStateMachine.I.ExitState<GameStateMachine.State_Dialogue>();
                    return true;
                case DialogueInputHandler.INPUT_ACTION_NAVIGATE:
                    if (!context.performed) { return true; }
                    if (!_WC_optionLister.IsActive) { break; }
                    Vector2 inputValue = context.ReadValue<Vector2>();
                    _WC_optionLister.Input_Navigate(inputValue);
                    return true;
            }

            return false;
        }

        public void SetActive(IState_Dialogue_Args args)
        {
            if (_isActive)
            {
                SetInactive();
            }
            _isActive = true;
            _args = args;
            if (args is State_Dialogue_PixelCrushers_Args pixelCrushersArgs)
            {
                MyLogger<DialogueController>.StaticLog($"Starting PixelCrushers conversation '{pixelCrushersArgs.ConversationTitle}' between '{pixelCrushersArgs.Actor.GetActorName()}' and '{pixelCrushersArgs.Conversant.GetActorName()}'");
                MyLogger<DialogueController>.StaticLog($"Current DialogueManager.dialogueUI: {DialogueManager.dialogueUI?.GetType().Name}");
                MyLogger<DialogueController>.StaticLog($"Is this the registered UI? {ReferenceEquals(DialogueManager.dialogueUI, this)}");
                DialogueManager.StartConversation(pixelCrushersArgs.ConversationTitle, pixelCrushersArgs.Actor.transform, pixelCrushersArgs.Conversant.transform);
            }
            else if (args is State_Dialogue_Args standardArgs)
            {
                ShowSubtitle(standardArgs.Dialogue, 2f, true);
            }
        }

        public void SetInactive()
        {
            if (!_isActive) { return; }
            _isActive = false;
            // Stop the Pixel Crushers Dialogue
            if (DialogueManager.isConversationActive) // If we were in a PixelCrushers conversation, stop it
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
            _VE_dialogContinueIcon.EnableInClassList(CLASS_DIALOG_CONTINUE_ICON_HIDDEN, true);
            _VE_dialogEndIcon.EnableInClassList(CLASS_DIALOG_END_ICON_HIDDEN, true);
            HideResponses();
            _VE_root.AddToClassList(CLASS_INACTIVE);
            _VE_optionListerParent.EnableInClassList(CLASS_OPTION_LISTER_PARENT_HIDDEN, true);
            // Forget the args
            _args = null;
            MyLogger<DialogueController>.StaticLog($"SetInactive()");
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

        public void Open()
        {
            if (!_isActive)
            {
                MyLogger<DialogueController>.StaticLogError("Open() was called but the Dialogue Controller isn't active.");
                return;
            }
            // Show UI
            _VE_root.RemoveFromClassList(CLASS_INACTIVE);
            // Handle nametag
            if (string.IsNullOrEmpty(_args.Nametag))
            {
                _VE_nametag.style.display = DisplayStyle.None;
            }
            else
            {
                _VE_nametag.style.display = DisplayStyle.Flex;
                _VE_nametagLabel.text = _args.Nametag;
            }
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
                _VE_nametagLabel.text = subtitle.speakerInfo.nameInDatabase;
                _VE_nametag.RemoveFromClassList(CLASS_INACTIVE);
            }
            else
            {
                _VE_nametag.AddToClassList(CLASS_INACTIVE);
            }

            // Check if this subtitle has a very short or "None()" sequence that would cause immediate hiding
            if (string.IsNullOrEmpty(subtitle.sequence) || subtitle.sequence.Contains("None()@0"))
            {
                MyLogger<DialogueController>.StaticLog($"Subtitle has short/empty sequence '{subtitle.sequence}', using custom timing");
                // Show text immediately and use our own timing
                _VE_dialogLabel.text = subtitle.formattedText.text;
                // Don't hide it immediately - wait for the next valid call
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
        }

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
            _VE_dialogEndIcon.EnableInClassList(CLASS_DIALOG_END_ICON_HIDDEN, true);
            _VE_dialogContinueIcon.EnableInClassList(CLASS_DIALOG_CONTINUE_ICON_HIDDEN, true);
            // Hide responses if any
            HideResponses();
            MyLogger<DialogueController>.StaticLog($"Showing subtitle: {text}");
            // Don't show empty text
            if (string.IsNullOrEmpty(text))
            {
                MyLogger<DialogueController>.StaticLog($"Empty text provided to ShowSubtitle, skipping animation");
                return;
            }
            // Display dialogue
            _VE_dialogLabel.text = string.Empty;
            _textAnimationTween = DOTween.To(() => _VE_dialogLabel.text, x => _VE_dialogLabel.text = x, text, animationDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _textAnimationTween = null;
                    // Display options if we have them
                    //ShowOptions();
                    if (_args is State_Dialogue_Args standardArgs && standardArgs.OptionsArgs != null)
                    {
                        ShowResponses(standardArgs.OptionsArgs.Options);
                    }
                    else if (_args is State_Dialogue_PixelCrushers_Args pcArgs)
                    {
                        // For Pixel Crushers, the system will automatically call ShowResponses when needed
                        MyLogger<DialogueController>.StaticLog("Text animation complete for Pixel Crushers dialogue");
                        // If this is the last subtitle, show the end icon, otherwise, just hide it using inline styles
                        if (isLast)
                        {
                            _VE_dialogEndIcon.EnableInClassList(CLASS_DIALOG_END_ICON_HIDDEN, false);
                            _VE_dialogContinueIcon.EnableInClassList(CLASS_DIALOG_CONTINUE_ICON_HIDDEN, true);
                        }
                        else
                        {
                            _VE_dialogEndIcon.EnableInClassList(CLASS_DIALOG_END_ICON_HIDDEN, true);
                            MyLogger<DialogueController>.StaticLog($"CLASS_DIALOG_CONTINUE_ICON_HIDDEN: {_COR_responses != null}");
                            _VE_dialogContinueIcon.EnableInClassList(CLASS_DIALOG_CONTINUE_ICON_HIDDEN, _COR_responses != null);
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
            _VE_dialogLabel.text = string.Empty;
            // Hide the nametag
            _VE_nametag.AddToClassList(CLASS_INACTIVE);
        }

        private void ShowResponses(string[] responses)
        {
            MyLogger<DialogueController>.StaticLog($"Showing responses: {string.Join(", ", responses)}");
            _VE_optionListerParent.EnableInClassList(CLASS_OPTION_LISTER_PARENT_HIDDEN, false);
            // Get reference to the option lister (Immediate child).
            var VE_optionLister = _VE_optionListerParent.ElementAt(0).ElementAt(0);
            _WC_optionLister.SetActive(
                VE_optionLister,
                options: responses,
                onOptionSelected: (index) =>
                {
                    MyLogger<DialogueController>.StaticLog($"Confirming selection: {index}");
                    if (_args is State_Dialogue_Args standardArgs)
                    {
                        var callback = standardArgs.OptionsArgs.Callback;
                        _args = null; // Clear args before invoking callback
                        callback?.Invoke(index);
                    }
                },
                onBackSelected: () =>
                {
                    MyLogger<DialogueController>.StaticLog($"Confirming selection: -1 (cancel option)");
                    if (_args is State_Dialogue_Args standardArgs)
                    {
                        var callback = standardArgs.OptionsArgs.Callback;
                        _args = null; // Clear args before invoking callback
                        callback?.Invoke(-1);
                    }
                }
            );
        }

        private IEnumerator Async_ShowResponses(Response[] responses)
        {
            while (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                yield return null;
            }

            MyLogger<DialogueController>.StaticLog($"Showing {responses.Length} responses...");
            _VE_optionListerParent.EnableInClassList(CLASS_OPTION_LISTER_PARENT_HIDDEN, false);

            // Get reference to the option lister (Immediate child).
            var VE_optionLister = _VE_optionListerParent.ElementAt(0).ElementAt(0);
            string[] responseLabels = new string[responses.Length];
            for (int i = 0; i < responses.Length; i++)
            {
                responseLabels[i] = responses[i].formattedText.text;
            }
            _WC_optionLister.SetActive(
                VE_optionLister,
                options: responseLabels,
                onOptionSelected: (index) =>
                {
                    MyLogger<DialogueController>.StaticLog($"Confirming selection: {index}");
                    // Use the IDialogueUI event handler for PixelCrushers integration
                    SelectedResponseHandler?.Invoke(this, new SelectedResponseEventArgs(responses[index]));
                },
                onBackSelected: () =>
                {
                    MyLogger<DialogueController>.StaticLog($"Confirming selection: -1 (cancel option). Don't know what to do in this case.");
                }
            );
            _COR_responses = null;
        }

        public void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
        {
            MyLogger<DialogueController>.StaticLog($"Stored {responses.Length} responses in the buffer.");
            if (_COR_responses != null)
            {
                StopCoroutine(_COR_responses);
            }
            _COR_responses = StartCoroutine(Async_ShowResponses(responses));
        }

        public void HideResponses()
        {
            MyLogger<DialogueController>.StaticLog("Hiding responses.");
            _VE_optionListerParent.EnableInClassList(CLASS_OPTION_LISTER_PARENT_HIDDEN, true);
            _WC_optionLister.SetInactive();
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
    }
}