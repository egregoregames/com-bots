using System;
using System.Collections.Generic;
using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using DG.Tweening;

namespace ComBots.Global.UI.Dialogue
{
    public class DialogueController : UIController, IInputHandler
    {
        protected override string UserInterfaceName => "Global.Dialogue";

        public override Dependency Dependency => Dependency.Independent;

        private const string CLASS_INACTIVE = "inactive";
        private const string CLASS_OPTION_LABEL = "option-label";
        private const string CLASS_OPTION = "option";
        private const string CLASS_OPTION_PARENT = "options";
        private const string CLASS_OPTION_HIGHLIGHTED = "option-highlighted";
        private const string CLASS_DIALOG_LABEL = "dialog-label";
        private const string CLASS_NAMETAG = "nametag";
        private const string CLASS_NAMETAG_LABEL = "nametag-label";
        private const string CLASS_OPTION_PARENT_HIDDEN = "options-hidden";
        private const string CLASS_OPTION_TOP_ROUNDED = "option-top-rounded";
        private const string CLASS_OPTION_BOTTOM_ROUNDED = "option-bottom-rounded";

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        private VisualElement _VE_root;

        [Header("Options")]
        [SerializeField] private VisualTreeAsset _VE_optionTemplate;
        private VisualElement _VE_optionParent;
        private List<OptionController> _optionControllers;
        private int _selectionIndex = 0;

        // Overflow arrows
        private VisualElement _VE_overflowTop;
        private VisualElement _VE_overflowBottom;

        // Scrolling system
        private const int MAX_VISIBLE_OPTIONS = 8;
        private int _totalOptions = 0;
        private int _scrollOffset = 0;

        // Dialog
        private Label _VE_dialogLabel;
        private VisualElement _VE_nametag;
        private Label _VE_nametagLabel;

        // Animation state
        private Tween _textAnimationTween;

        // Events
        private State_Dialogue_Args _args;

        protected override void Init()
        {
            _optionControllers = new();
            _VE_root = _uiDocument.rootVisualElement.Q(name: "Root");
            _VE_optionParent = _VE_root.Q<VisualElement>(className: CLASS_OPTION_PARENT);
            _VE_dialogLabel = _VE_root.Q<Label>(className: CLASS_DIALOG_LABEL);
            _VE_nametag = _VE_root.Q(className: CLASS_NAMETAG);
            _VE_nametagLabel = _VE_root.Q<Label>(className: CLASS_NAMETAG_LABEL);

            // Get overflow arrow elements
            _VE_overflowTop = _VE_root.Q<VisualElement>("OptionOverflowTop");
            _VE_overflowBottom = _VE_root.Q<VisualElement>("OptionOverflowBottom");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_optionTemplate = null;
            _optionControllers.Clear();
            _optionControllers = null;
            _args = null;

            GlobalConfig.I.InputSO.OnUp -= Input_Up;
            GlobalConfig.I.InputSO.OnDown -= Input_Down;
        }

        /// <summary>
        /// This must be called by the state machine when the dialogue state is exited.
        /// </summary>
        public void OnExit()
        {
            //Deselect current option using visual index
            int visualIndex = _selectionIndex - _scrollOffset;
            if (visualIndex >= 0 && visualIndex < _optionControllers.Count)
            {
                _optionControllers[visualIndex].VE?.RemoveFromClassList(CLASS_OPTION_HIGHLIGHTED);
            }

            // Invoke the callback if available
            if (_args != null)
            {
                if (_args.OptionsArgs != null && _args.OptionsArgs.Callback != null)
                {
                    _args.OptionsArgs.Callback.Invoke(_selectionIndex);
                }
                _args = null;
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            // Ignore input while text is animating
            if (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                return true; // Consume the input but don't process it
            }

            switch (actionName)
            {
                case DialogueInputHandler.INPUT_ACTION_NAVIGATE:
                    if (context.performed)
                    {
                        Vector2 inputValue = context.ReadValue<Vector2>();
                        if (inputValue.y > 0)
                        {
                            Input_Up();
                        }
                        else if (inputValue.y < 0)
                        {
                            Input_Down();
                        }
                    }
                    return true;
                case DialogueInputHandler.INPUT_ACTION_CANCEL:
                    if (context.performed)
                    {
                        Input_Cancel();
                    }
                    return true;
                case DialogueInputHandler.INPUT_ACTION_CONFIRM:
                    if (context.performed)
                    {
                        Input_Confirm();
                    }
                    return true;
            }

            return false;
        }

        public void SetActive(State_Dialogue_Args args)
        {
            _args = args;
            // Display dialogue
            _VE_dialogLabel.text = string.Empty;
            _textAnimationTween = DOTween.To(() => _VE_dialogLabel.text, x => _VE_dialogLabel.text = x, args.Dialogue, 1f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _textAnimationTween = null;
                    // If options are available, show them
                    if (_args.OptionsArgs != null)
                    {
                        _VE_optionParent.RemoveFromClassList(CLASS_OPTION_PARENT_HIDDEN);
                    }
                });
            // Handle nametag
            if (string.IsNullOrEmpty(args.Nametag))
            {
                _VE_nametag.style.display = DisplayStyle.None;
            }
            else
            {
                _VE_nametag.style.display = DisplayStyle.Flex;
                _VE_nametagLabel.text = args.Nametag;
            }
            // Handle options
            _VE_optionParent.AddToClassList(CLASS_OPTION_PARENT_HIDDEN); // Make sure options are hidden initially
            if (args.OptionsArgs != null)
            {
                _totalOptions = args.OptionsArgs.Options.Length + 1; // +1 for cancel option
                _scrollOffset = 0;
                _selectionIndex = 0;

                // Create visual elements (limited to MAX_VISIBLE_OPTIONS)
                int visibleCount = Mathf.Min(_totalOptions, MAX_VISIBLE_OPTIONS);
                SetOptionCount(visibleCount);

                // Setup initial visible options
                UpdateVisibleOptions();
            }
            else
            {
                // Hide options
                _VE_optionParent.AddToClassList(CLASS_OPTION_PARENT_HIDDEN);
            }
            // Show UI
            _VE_root.RemoveFromClassList(CLASS_INACTIVE);
            MyLogger<DialogueController>.StaticLog($"Room selection active with {args.OptionsArgs?.Options.Length ?? 0} options.");
        }

        public void SetInactive()
        {
            _VE_root.AddToClassList(CLASS_INACTIVE);

            // Kill the text animation if it's still running
            if (_textAnimationTween != null && _textAnimationTween.IsActive())
            {
                _textAnimationTween.Kill();
                _textAnimationTween = null;
            }

            MyLogger<DialogueController>.StaticLog($"Room selection inactive.");
        }

        private void Input_Cancel()
        {
            _selectionIndex = 0; // Reset to cancel option
            GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
        }

        private void Input_Confirm()
        {
            // Check if this dialogue has options
            if (_args != null && _args.OptionsArgs != null && _args.OptionsArgs.Callback != null)
            {
                if (_selectionIndex != 0) // If not the cancel option
                {
                    var args = _args;
                    _args = null; // Clear args after handling so that OnExit doesn't invoke the callback again
                    args.OptionsArgs.Callback.Invoke(_selectionIndex);
                    return;
                } // If cancel option is selected, just go back to playing state
            }

            // Go back to playing state
            GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
        }

        private void Input_Up()
        {
            int newIndex = _selectionIndex + 1;
            if (newIndex >= _totalOptions)
            {
                newIndex = 0;
                _scrollOffset = 0;
            }
            else if (newIndex - _scrollOffset >= MAX_VISIBLE_OPTIONS)
            {
                _scrollOffset++;
            }

            SetSelected(newIndex);
            UpdateVisibleOptions();
        }

        private void Input_Down()
        {
            int newIndex = _selectionIndex - 1;
            if (newIndex < 0)
            {
                newIndex = _totalOptions - 1;
                _scrollOffset = Mathf.Max(0, _totalOptions - MAX_VISIBLE_OPTIONS);
            }
            else if (newIndex < _scrollOffset)
            {
                _scrollOffset--;
            }

            SetSelected(newIndex);
            UpdateVisibleOptions();
        }

        private void SetSelected(int index)
        {
            // Remove highlighting from current selection (using visual index)
            int currentVisualIndex = _selectionIndex - _scrollOffset;
            if (currentVisualIndex >= 0 && currentVisualIndex < _optionControllers.Count)
            {
                _optionControllers[currentVisualIndex].VE.RemoveFromClassList(CLASS_OPTION_HIGHLIGHTED);
            }

            _selectionIndex = index;

            // Add highlighting to new selection (using visual index)
            int newVisualIndex = _selectionIndex - _scrollOffset;
            if (newVisualIndex >= 0 && newVisualIndex < _optionControllers.Count)
            {
                _optionControllers[newVisualIndex].VE.AddToClassList(CLASS_OPTION_HIGHLIGHTED);
            }
        }

        private void UpdateVisibleOptions()
        {
            if (_args?.OptionsArgs == null) return;

            // Update each visible option controller with the correct data
            for (int i = 0; i < _optionControllers.Count; i++)
            {
                // Clear classes for this option
                _optionControllers[i].VE.RemoveFromClassList(CLASS_OPTION_HIGHLIGHTED);

                int dataIndex = i + _scrollOffset;

                if (dataIndex == 0)
                {
                    // Cancel option (always at index 0)
                    _optionControllers[i].Setup(0, _args.OptionsArgs.CancelOption);
                }
                else if (dataIndex - 1 < _args.OptionsArgs.Options.Length)
                {
                    // Regular option (subtract 1 because cancel option takes index 0)
                    _optionControllers[i].Setup(dataIndex, _args.OptionsArgs.Options[dataIndex - 1]);
                }
            }

            // Update overflow arrows
            UpdateOverflowArrows();

            // Update selection highlighting
            int visualIndex = _selectionIndex - _scrollOffset;
            if (visualIndex >= 0 && visualIndex < _optionControllers.Count)
            {
                _optionControllers[visualIndex].VE.AddToClassList(CLASS_OPTION_HIGHLIGHTED);
            }
        }

        private void UpdateOverflowArrows()
        {
            bool hasOptionsBelow = _scrollOffset > 0;
            bool hasOptionsAbove = _scrollOffset + _optionControllers.Count < _totalOptions;

            // Show/hide overflow arrows
            _VE_overflowTop.style.display = hasOptionsAbove ? DisplayStyle.Flex : DisplayStyle.None;
            _VE_overflowBottom.style.display = hasOptionsBelow ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void SetOptionCount(int count)
        {
            if (_optionControllers.Count > count)
            {
                for (int i = _optionControllers.Count - 1; i >= count; i--)
                {
                    _optionControllers[i].VE = null;
                    _VE_optionParent.Remove(_optionControllers[i].VE);
                    _optionControllers.RemoveAt(i);
                }
            }
            else if (_optionControllers.Count < count)
            {
                if (_optionControllers.Count > 0)
                {
                    _optionControllers[^1].VE.RemoveFromClassList(CLASS_OPTION_TOP_ROUNDED);
                }
                for (int i = _optionControllers.Count; i < count; i++)
                {
                    InstantiateOption();
                }
            }
            //Set rounded corners
            if (!_optionControllers[0].VE.ClassListContains(CLASS_OPTION_BOTTOM_ROUNDED))
            {
                _optionControllers[0].VE.AddToClassList(CLASS_OPTION_BOTTOM_ROUNDED);
            }
            if (!_optionControllers[^1].VE.ClassListContains(CLASS_OPTION_TOP_ROUNDED))
            {
                _optionControllers[^1].VE.AddToClassList(CLASS_OPTION_TOP_ROUNDED);
            }
            // Move overflow arrows the the bottom of the childen list
            _VE_optionParent.Remove(_VE_overflowBottom);
            _VE_optionParent.Add(_VE_overflowBottom);
            _VE_optionParent.Remove(_VE_overflowTop);
            _VE_optionParent.Add(_VE_overflowTop);
        }

        private void InstantiateOption()
        {
            VisualElement VE_option = _VE_optionTemplate.Instantiate().contentContainer;
            VE_option.AddToClassList(CLASS_OPTION);
            _VE_optionParent.Add(VE_option);
            _optionControllers.Add(new(VE_option));
        }

        private class OptionController
        {
            public VisualElement VE;
            public int index;

            public OptionController(VisualElement VE)
            {
                this.VE = VE;
                this.index = -1;
            }

            public void Setup(int index, string label)
            {
                this.index = index;
                VE.Q<Label>(className: CLASS_OPTION_LABEL).text = label;
            }
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
    }
}