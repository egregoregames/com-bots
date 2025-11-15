using ComBots.Game.StateMachine;
using ComBots.Logs;
using ComBots.UI.Dialogue;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using R3;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WC_Dialogue : MonoBehaviourR3, IDialogueUI
{
    private bool _isActive;

    public static WC_Dialogue Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject _w_root;

    [Header("Dialogue")]
    [SerializeField] private WC_Typewriter _dialogueTypewriter;

    [Header("Utility Lister")]
    [SerializeField] private WC_Lister<WC_DialogueOption> _optionLister;

    [Header("Dialogue Events")]
    [SerializeField] private DialogueSystemEvents _dialogueSystemEvents;

    [Header("NameTag")]
    [SerializeField] private GameObject _nametag;
    [SerializeField] private Image _nametagIcon;
    [SerializeField] private TextMeshProUGUI _nametagText;

    [Header("End & Continue Icons")]
    [SerializeField] private float _iconSpeed;
    [SerializeField] private RectTransform _continueIcon;
    [SerializeField] private float _continueIcon_moveAmount;
    [SerializeField] private Transform _endIcon;
    [SerializeField] private float _endIcon_scaleAmount;

    [Header("Sound Effects")]
    [SerializeField] private DialogueSoundEffects _defaultSFX;
    [SerializeField] private TypeWriterSoundEffects _typeWriterSFX;

    // ============ Responses ============ //
    private Response[] _pcArgs_responsesBuffer;
    private Coroutine _COR_responses;

    // =============== Public Events =============== //
    public UnityAction OnDialogueStarted;
    public UnityAction OnDialogueEnded;
    public bool IsDialogueActive => _isActive;

    // =============== Cache =============== //
    private IState_Dialogue_Args _args;
    private bool _isFirstSubtitle;
    private bool _isLastSubtitle;
    private bool _previouslyHadOptions;

    // =============== IDialogueUI implementation =============== //
    public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;

    private InputSystem_Actions Inputs { get; set; }

    private new void Awake()
    {
        base.Awake();
        _w_root.SetActive(false);
    }

    #region Initialization & Disposal
    protected override void Initialize()
    {
        base.Initialize();
        Instance = this;
        Inputs = new();

        // Register this as the DialogueManager's UI
        DialogueManager.dialogueUI = this;

        // Check Display Settings
        if (DialogueManager.displaySettings != null)
        {
            MyLogger<WC_Dialogue>.StaticLog($"Display Settings found. Checking subtitle settings...");
            MyLogger<WC_Dialogue>.StaticLog($"DialogueManager.displaySettings.subtitleSettings type: {DialogueManager.displaySettings.subtitleSettings?.GetType().Name}");
        }
        else
        {
            MyLogger<WC_Dialogue>.StaticLogWarning("DialogueManager.displaySettings is null!");
        }

        // These can probably go in Awake
        // ============ Animate Continue Icon ============ //
        float halfMove = _continueIcon_moveAmount * 0.5f;
        Vector2 startPos = _continueIcon.anchoredPosition + Vector2.down * halfMove;
        Vector2 targetPos = startPos + Vector2.up * _continueIcon_moveAmount;
        _continueIcon.anchoredPosition = startPos;
        _continueIcon.DOAnchorPosY(targetPos.y, _iconSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // ============ Animate End Icon ============ //
        Vector3 startScale = _endIcon.localScale;
        Vector3 targetScale = startScale * _endIcon_scaleAmount;
        _endIcon.DOScale(targetScale, _iconSpeed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        var onCancel = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Cancel.performed += h,
            h => Inputs.UI.Cancel.performed -= h);

        var onSubmit = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Submit.performed += h,
            h => Inputs.UI.Submit.performed -= h);

        var onNavigate = Observable.FromEvent<InputAction.CallbackContext>(
            h => Inputs.UI.Navigate.performed += h,
            h => Inputs.UI.Navigate.performed -= h);

        AddEvents(
            onCancel.Subscribe(OnCancel),
            onSubmit.Subscribe(OnSubmit),
            onNavigate.Subscribe(OnNavigate));
    }

    private void OnNavigate(InputAction.CallbackContext context)
    {
        if (_dialogueTypewriter.IsTyping)
            return;

        if (!_optionLister.IsActive)
            return;

        AudioManager.PlaySoundEffect(_defaultSFX.NavigateOptions);
        Vector2 inputValue = context.ReadValue<Vector2>();
        _optionLister.Input_Navigate(inputValue);
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (_dialogueTypewriter.IsTyping) return;

        if (_optionLister.IsActive) // If we have options pass the input
        {
            if (!_isLastSubtitle)
            {
                AudioManager.PlaySoundEffect(_defaultSFX.ChooseOption);
            }
            _optionLister.Input_Confirm();
            return;
        }

        MyLogger<WC_Dialogue>.StaticLog("Advancing PixelCrushers conversation.");
        DialogueManager.instance.SendMessage(DialogueSystemMessages.OnConversationContinue, (IDialogueUI)this, SendMessageOptions.DontRequireReceiver);

        // Maybe this could listen to a conversation ended event
        // GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (_dialogueTypewriter.IsTyping) return;
        //GameStateMachine.I.ExitState<GameStateMachine.State_Dialogue>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        Inputs.Enable();
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }

    private new void OnDestroy()
    {
        base.OnDestroy();
        _optionLister?.Dispose();
    }
    #endregion

    #region State Machine API

    public void SetActive(IState_Dialogue_Args args)
    {
        if (_isActive)
        {
            SetInactive();
        }
        _isActive = true;
        _previouslyHadOptions = false;
        _args = args;
        _isFirstSubtitle = true;

        // Hide icons at start
        _endIcon.gameObject.SetActive(false);
        _continueIcon.gameObject.SetActive(false);

        if (args is State_Dialogue_PixelCrushers_Args pixelCrushersArgs)
        {
            MyLogger<WC_Dialogue>.StaticLog($"Starting PixelCrushers conversation '{pixelCrushersArgs.ConversationTitle}' between '{pixelCrushersArgs.Actor.GetActorName()}' and '{pixelCrushersArgs.Conversant.GetActorName()}'");
            MyLogger<WC_Dialogue>.StaticLog($"Current DialogueManager.dialogueUI: {DialogueManager.dialogueUI?.GetType().Name}");
            MyLogger<WC_Dialogue>.StaticLog($"Is this the registered UI? {ReferenceEquals(DialogueManager.dialogueUI, this)}");
            DialogueManager.StartConversation(pixelCrushersArgs.ConversationTitle, pixelCrushersArgs.Actor.transform, pixelCrushersArgs.Conversant.transform);
            // DisplayGUI() Will be called automatically by DialogueManager using Open()
        }
        else if (args is State_Dialogue_Args standardArgs)
        {
            ShowSubtitle(standardArgs.Dialogue, true, false);
            DisplayGUI();
        }
    }

    public void SetInactive()
    {
        if (!_isActive) { return; }
        _isActive = false;
        // SFX
        AudioManager.PlaySoundEffect(_defaultSFX.EndDialogue);
        // Stop the Pixel Crushers Dialogue
        if (DialogueManager.isConversationActive)
        {
            MyLogger<WC_Dialogue>.StaticLog($"PixelCrushers.DialogueManager.StopConversation()");
            DialogueManager.StopConversation();
        }
        _dialogueTypewriter.SetInactive(true, false);
        // Hide Dialogue UI
        _continueIcon.gameObject.SetActive(false);
        _endIcon.gameObject.SetActive(false);
        HideResponses();
        _w_root.SetActive(false);
        // Clear args
        _args = null;
        MyLogger<WC_Dialogue>.StaticLog($"SetInactive()");
        // Events
        OnDialogueEnded?.Invoke();
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
        MyLogger<WC_Dialogue>.StaticLog($"ShowSubtitle called - Title: '{subtitle.dialogueEntry.Title}', Actor: '{subtitle.speakerInfo.nameInDatabase}', Text: '{subtitle.formattedText.text}', Sequence: '{subtitle.sequence}'");

        // If it's empty text (like the START node), continue immediately
        if (string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            MyLogger<WC_Dialogue>.StaticLog($"Empty subtitle, continuing...");
            StartCoroutine(ContinueAfterFrame());
            return;
        }

        // Only skip if this is actually a player response that we want to skip
        if (subtitle.speakerInfo.nameInDatabase == "Player")
        {
            MyLogger<WC_Dialogue>.StaticLog($"Skipping player response: {subtitle.dialogueEntry.DialogueText}");
            StartCoroutine(ContinueAfterFrame());
            return;
        }

        // For NPC dialogue, show it
        MyLogger<WC_Dialogue>.StaticLog($"Displaying NPC dialogue from {subtitle.speakerInfo.nameInDatabase}: {subtitle.formattedText.text}");

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
            MyLogger<WC_Dialogue>.StaticLog($"Subtitle has short/empty sequence '{subtitle.sequence}', using custom timing");
            _dialogueTypewriter.SetActive(subtitle.formattedText.text, null, _typeWriterSFX);
        }
        else
        {
            AnalyzeSubtitle(subtitle, out bool isLast, out bool _);
            ShowSubtitle(subtitle.formattedText.text, isLast, _previouslyHadOptions);
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
            MyLogger<WC_Dialogue>.StaticLogError($"Error analyzing subtitle: {ex.Message}");
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

    private async void ShowSubtitle(string text, bool isLast, bool previouslyHadOptions)
    {
        MyLogger<WC_Dialogue>.StaticLog($"Showing subtitle: {text}, isfirst: {_isFirstSubtitle}, isLast: {isLast}, previouslyHadOptions: {previouslyHadOptions}");
        // Cache
        _isLastSubtitle = isLast;

        // Play the SFX if this isn't the first or last subtitle
        if (!_isFirstSubtitle && !previouslyHadOptions)
        {
            AudioManager.PlaySoundEffect(_defaultSFX.ContinueDialogue);
        }

        if (_isFirstSubtitle)
        {
            _isFirstSubtitle = false;
        }

        // Hide end & continue icons
        _endIcon.gameObject.SetActive(false);
        _continueIcon.gameObject.SetActive(false);
        // Hide responses if any
        HideResponses();

        // Don't show empty text
        if (string.IsNullOrEmpty(text))
        {
            MyLogger<WC_Dialogue>.StaticLog($"Empty text provided to ShowSubtitle, skipping animation");
            return;
        }

        // ============ Substitute values ============ //
        var gameData = await PersistentGameData.GetInstanceAsync();

        string playerName = gameData.PlayerName;
        string basicBot = "FlameBot"; // Default fallback
        if (gameData.PlayerTeamBotStatusData.Count > 0)
        {
            basicBot = gameData.PlayerTeamBotStatusData[0].BlueprintId ?? "FlameBot";
        }
        // Replace player name
        text = text.Replace("${varName}", playerName);
        text = text.Replace("${varBasicBot.name}", basicBot);

        // Display dialogue
        _dialogueTypewriter.SetActive(text, () =>
        {
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
                    MyLogger<WC_Dialogue>.StaticLog($"Showing end icon for last subtitle.");
                    _endIcon.gameObject.SetActive(true);
                    _continueIcon.gameObject.SetActive(false);
                }
                else
                {
                    MyLogger<WC_Dialogue>.StaticLog($"Showing continue icon for non-last subtitle.");
                    _endIcon.gameObject.SetActive(false);
                    _continueIcon.gameObject.SetActive(_COR_responses == null);
                }
            }
        }, _typeWriterSFX);
    }

    public void HideSubtitle(Subtitle subtitle)
    {
        MyLogger<WC_Dialogue>.StaticLog($"HideSubtitle called for: '{subtitle.formattedText.text}' from speaker: '{subtitle.speakerInfo.nameInDatabase}'");

        // Only hide if this is not an NPC subtitle that should stay visible
        if (subtitle.speakerInfo.nameInDatabase != "Player" && !string.IsNullOrEmpty(subtitle.formattedText.text))
        {
            MyLogger<WC_Dialogue>.StaticLog($"Keeping NPC subtitle visible for responses");
            // Don't clear NPC subtitles immediately - they should stay visible during response selection
            return;
        }

        // Clear the dialogue text for empty subtitles or player text
        _dialogueTypewriter.SetInactive(true, false);

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
        MyLogger<WC_Dialogue>.StaticLog("Hiding responses.");
        _previouslyHadOptions = _optionLister.IsActive;
        _optionLister.SetInactive();
        _COR_responses = null;
    }

    public void ShowQTEIndicator(int index)
    {
        MyLogger<WC_Dialogue>.StaticLogWarning($"DialogueController.ShowQTEIndicator() is not implemented for index {index}.");
    }

    public void HideQTEIndicator(int index)
    {
        MyLogger<WC_Dialogue>.StaticLogWarning($"DialogueController.HideQTEIndicator() is not implemented for index {index}.");
    }

    public void ShowAlert(string message, float duration)
    {
        MyLogger<WC_Dialogue>.StaticLogWarning("DialogueController.ShowAlert() is not implemented.");
    }

    public void HideAlert()
    {
        MyLogger<WC_Dialogue>.StaticLogWarning("DialogueController.HideAlert() is not implemented.");
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
        MyLogger<WC_Dialogue>.StaticLog($"Confirming selection: {index}");
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
        MyLogger<WC_Dialogue>.StaticLog($"Showing responses: {string.Join(", ", responses)}");
        _optionLister.SetActive(
            responses.Length,
            OptionLister_SetupOption,
            onOptionSelected: OptionLister_OnSelected,
            true
        );
    }

    private IEnumerator Async_PCArgs_ShowResponses(Response[] responses)
    {
        while (_dialogueTypewriter.IsTyping)
        {
            yield return null;
        }

        MyLogger<WC_Dialogue>.StaticLog($"Showing {responses.Length} responses...");

        if (_pcArgs_responsesBuffer == null || _pcArgs_responsesBuffer.Length != responses.Length)
        {
            _pcArgs_responsesBuffer = new Response[responses.Length];
        }
        for (int i = 0; i < responses.Length; i++)
        {
            _pcArgs_responsesBuffer[i] = responses[i];
        }
        MyLogger<WC_Dialogue>.StaticLog($"Stored {responses.Length} responses in the buffer.");
        _optionLister.SetActive(
            responses.Length,
            OptionLister_SetupOption,
            OptionLister_OnSelected,
            false
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
            MyLogger<WC_Dialogue>.StaticLogError("Open() was called but the Dialogue Controller isn't active.");
            return;
        }
        OnDialogueStarted?.Invoke();

        // Show UI
        _w_root.SetActive(true);

        // Ensure icons are properly hidden when opening dialogue
        _endIcon.gameObject.SetActive(false);
        _continueIcon.gameObject.SetActive(false);

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