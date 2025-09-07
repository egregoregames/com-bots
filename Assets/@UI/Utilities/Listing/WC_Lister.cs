using System;
using System.Collections.Generic;
using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.UI.Utilities
{
    /// <summary>
    /// A reusable utility for creating scrollable lists with input navigation.
    /// Extracted from the dialogue system for general use throughout the project.
    /// </summary>
    public class WC_Lister : UIController
    {
        protected override string UserInterfaceName => "Utility.Listing";

        public override Dependency Dependency => Dependency.Independent;

        // CSS class constants
        private const string CLASS_OPTION_LABEL = "utility-lister-option-label";
        private const string CLASS_OPTION = "utility-lister-option";
        private const string CLASS_OPTION_PARENT = "utility-lister";
        private const string CLASS_OPTION_HIGHLIGHTED = "utility-lister-option-highlighted";
        private const string CLASS_OPTION_TOP_ROUNDED = "utility-lister-option-top-rounded";
        private const string CLASS_OPTION_BOTTOM_ROUNDED = "utility-lister-option-bottom-rounded";
        private const string CLASS_OPTION_OVERFLOW_TOP = "utility-lister-option-overflow-top";
        private const string CLASS_OPTION_OVERFLOW_BOTTOM = "utility-lister-option-overflow-bottom";

        [Header("UI Configuration")]
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private VisualTreeAsset _optionTemplate;

        [Header("Settings")]
        [SerializeField] private int _maxVisibleOptions = 8;

        // UI Elements
        private VisualElement _optionParent;
        private VisualElement _overflowTop;
        private VisualElement _overflowBottom;

        // Option management
        private List<OptionController> _optionControllers;
        private List<string> _currentOptions;
        private int _selectionIndex = 0;
        private int _totalOptions = 0;
        private int _scrollOffset = 0;
        private bool _hasBackOption = false;

        // Callbacks
        private Action<int> _onOptionSelected;
        private Action _onBackSelected;

        public bool IsActive { get; private set; }

        protected override void Init()
        {
            MyLogger<WC_Lister>.StaticLog($"Init()");
            _optionControllers = new();
            _currentOptions = new();
        }

        public override void Dispose()
        {
            _optionControllers?.Clear();
            _currentOptions?.Clear();
            _onOptionSelected = null;
            _onBackSelected = null;
            _optionParent = null;
            _overflowTop = null;
            _overflowBottom = null;
        }

        /// <summary>
        /// Shows the lister with the specified options
        /// </summary>
        /// <param name="widget">The visual element container for the lister</param>
        /// <param name="options">Array of option strings to display</param>
        /// <param name="onOptionSelected">Callback when an option is selected (index parameter)</param>
        /// <param name="onBackSelected">Callback when back is selected (optional)</param>
        /// <param name="backOptionLabel">Label for back option, null to disable back option</param>
        public void SetActive(VisualElement widget, string[] options, Action<int> onOptionSelected, Action onBackSelected = null, string backOptionLabel = null)
        {
            MyLogger<WC_Lister>.StaticLog($"WC_Lister: SetActive called with {options?.Length ?? 0} options");
            if (options == null || options.Length == 0)
            {
                MyLogger<WC_Lister>.StaticLogError("UtilityListerController: Cannot show lister with null or empty options");
                return;
            }

            // Get UI elements
            _optionParent = widget.Q<VisualElement>(className: CLASS_OPTION_PARENT);
            _overflowTop = widget.Q<VisualElement>(className: CLASS_OPTION_OVERFLOW_TOP);
            _overflowBottom = widget.Q<VisualElement>(className: CLASS_OPTION_OVERFLOW_BOTTOM);

            // Set callbacks
            _onOptionSelected = onOptionSelected;
            _onBackSelected = onBackSelected;

            // Prepare options list
            _currentOptions.Clear();
            _hasBackOption = !string.IsNullOrEmpty(backOptionLabel);

            if (_hasBackOption)
            {
                _currentOptions.Add(backOptionLabel);
            }

            _currentOptions.AddRange(options);
            _totalOptions = _currentOptions.Count;

            // Reset selection and scrolling
            _selectionIndex = 0;
            _scrollOffset = 0;

            // Create visual elements
            int visibleCount = Mathf.Min(_totalOptions, _maxVisibleOptions);
            SetOptionCount(visibleCount);

            // Update display
            UpdateVisibleOptions();

            IsActive = true;
        }

        public void SetInactive()
        {
            IsActive = false;
        }

        public bool Input_Navigate(Vector2 direction)
        {
            if (!IsActive) return false;

            if (direction.y > 0)
            {
                Input_Up();
            }
            else if (direction.y < 0)
            {
                Input_Down();
            }

            return true;
        }

        public void Input_Cancel()
        {
            if (!IsActive)
            {
                MyLogger<WC_Lister>.StaticLogError("Input_Cancel called while lister is not active");
                return;
            }
            if (_hasBackOption)
            {
                _selectionIndex = 0; // Select back option
                Input_Confirm(); // Execute back action
            }
            else
            {
                _onBackSelected?.Invoke();
            }
        }

        public void Input_Confirm()
        {
            if (!IsActive)
            {
                MyLogger<WC_Lister>.StaticLogError("Input_Confirm called while lister is not active");
                return;
            }
            if (_hasBackOption && _selectionIndex == 0)
            {
                // Back option selected
                _onBackSelected?.Invoke();
            }
            else
            {
                // Regular option selected
                int actualIndex = _hasBackOption ? _selectionIndex - 1 : _selectionIndex;
                _onOptionSelected?.Invoke(actualIndex);
            }
        }

        public void Input_Down()
        {
            if (!IsActive)
            {
                MyLogger<WC_Lister>.StaticLogError("Input_Down called while lister is not active");
                return;
            }

            int newIndex = _selectionIndex + 1;
            if (newIndex >= _totalOptions)
            {
                newIndex = 0;
                _scrollOffset = 0;
            }
            else if (newIndex - _scrollOffset >= _maxVisibleOptions)
            {
                _scrollOffset++;
            }

            SetSelected(newIndex);
            UpdateVisibleOptions();
        }

        public void Input_Up()
        {
            if (!IsActive)
            {
                MyLogger<WC_Lister>.StaticLogError("Input_Up called while lister is not active");
                return;
            }
            int newIndex = _selectionIndex - 1;
            if (newIndex < 0)
            {
                newIndex = _totalOptions - 1;
                _scrollOffset = Mathf.Max(0, _totalOptions - _maxVisibleOptions);
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
            MyLogger<WC_Lister>.StaticLog($"Setting selection to index {index} (current is {_selectionIndex})");
            // Remove highlighting from current selection
            int currentVisualIndex = _selectionIndex - _scrollOffset;
            if (currentVisualIndex >= 0 && currentVisualIndex < _optionControllers.Count)
            {
                MyLogger<WC_Lister>.StaticLog($"Removing highlight from index {currentVisualIndex}");
                _optionControllers[currentVisualIndex].VE.RemoveFromClassList(CLASS_OPTION_HIGHLIGHTED);
            }

            _selectionIndex = index;

            // Add highlighting to new selection
            int newVisualIndex = _selectionIndex - _scrollOffset;
            if (newVisualIndex >= 0 && newVisualIndex < _optionControllers.Count)
            {
                MyLogger<WC_Lister>.StaticLog($"Adding highlight to index {newVisualIndex}");
                _optionControllers[newVisualIndex].VE.AddToClassList(CLASS_OPTION_HIGHLIGHTED);
            }
        }

        private void UpdateVisibleOptions()
        {
            if (_currentOptions == null || _currentOptions.Count == 0) return;

            // Update each visible option controller with the correct data
            for (int i = 0; i < _optionControllers.Count; i++)
            {
                // Clear highlighting
                _optionControllers[i].VE.RemoveFromClassList(CLASS_OPTION_HIGHLIGHTED);

                int dataIndex = i + _scrollOffset;
                if (dataIndex < _currentOptions.Count)
                {
                    _optionControllers[i].Setup(dataIndex, _currentOptions[dataIndex]);
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
            bool hasOptionsAbove = _scrollOffset > 0;
            bool hasOptionsBelow = _scrollOffset + _optionControllers.Count < _totalOptions;

            _overflowTop.style.display = hasOptionsAbove ? DisplayStyle.Flex : DisplayStyle.None;
            _overflowBottom.style.display = hasOptionsBelow ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void SetOptionCount(int count)
        {
            MyLogger<WC_Lister>.StaticLog($"Setting option count to {count} (current is {_optionControllers.Count})");
            // Remove excess options
            if (_optionControllers.Count > count)
            {
                for (int i = _optionControllers.Count - 1; i >= count; i--)
                {
                    _optionParent.Remove(_optionControllers[i].VE);
                    _optionControllers[i].VE = null;
                    _optionControllers.RemoveAt(i);
                }
            }
            // Add missing options
            else if (_optionControllers.Count < count)
            {
                if (_optionControllers.Count > 0)
                {
                    _optionControllers[^1].VE.RemoveFromClassList(CLASS_OPTION_BOTTOM_ROUNDED);
                }

                for (int i = _optionControllers.Count; i < count; i++)
                {
                    InstantiateOption();
                }
            }

            // Set rounded corners
            if (_optionControllers.Count > 0)
            {
                _optionControllers[0].VE.AddToClassList(CLASS_OPTION_TOP_ROUNDED);
                _optionControllers[^1].VE.AddToClassList(CLASS_OPTION_BOTTOM_ROUNDED);
            }

            // Ensure overflow arrows are at the end
            _optionParent.Remove(_overflowBottom);
            _optionParent.Add(_overflowBottom);
            _optionParent.Remove(_overflowTop);
            _optionParent.Add(_overflowTop);
        }

        private void InstantiateOption()
        {
            VisualElement optionElement = _optionTemplate.Instantiate().contentContainer;
            optionElement.AddToClassList(CLASS_OPTION);
            _optionParent.Add(optionElement);
            _optionControllers.Add(new OptionController(optionElement));
        }

        private class OptionController
        {
            public VisualElement VE;
            public int Index;

            public OptionController(VisualElement ve)
            {
                VE = ve;
                Index = -1;
            }

            public void Setup(int index, string label)
            {
                Index = index;
                VE.Q<Label>(className: CLASS_OPTION_LABEL).text = label;
            }
        }
    }
}
