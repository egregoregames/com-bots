using System;
using System.Collections.Generic;
using ComBots.Logs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.UI.Utilities.Listing
{
    /// <summary>
    /// A reusable utility for creating scrollable lists with input navigation.
    /// Extracted from the dialogue system for general use throughout the project.
    /// </summary>
    [System.Serializable]
    public class WC_Lister<WC_OptionT> where WC_OptionT : WC_ListerOption
    {
        // ============ Settings ============ //
        [Header("Listing Settings")]
        [SerializeField] private int _maxVisibleOptions = 8;

        // ============ Overflow Arrows ============ //
        [Header("Overflow Arrows")]
        [SerializeField] private GameObject _overflowTop;
        [SerializeField] private GameObject _overflowBottom;

        // ============ Options ============ //
        [Header("Options")]
        [SerializeField] private Transform _optionParent;
        [SerializeField] private WC_OptionT _optionPrefab;
        private List<WC_OptionT> _optionControllers;
        private bool _hasBackOption = false;
        private int _selectionIndex = 0;
        private int _totalOptions = 0;
        private int _scrollOffset = 0;

        // ============ Callbacks ============ //
        private Action<int> _onOptionSelected;
        private UnityAction<WC_OptionT, int> _SetupOption;

        public bool IsActive { get; private set; }

        // ----------------------------------------
        // Creation & Freeing
        // ----------------------------------------
        #region Creation & Freeing

        public WC_Lister()
        {
            _optionControllers = new();
        }

        public void Dispose()
        {
            _optionControllers?.Clear();
            _onOptionSelected = null;
            _optionParent = null;
            _overflowTop = null;
            _overflowBottom = null;
        }

        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        /// <summary>
        /// Shows the lister with the specified options
        /// </summary>
        /// <param name="count">Number of options to display</param>
        /// <param name="SetupOption">Callback to setup each option</param>
        /// <param name="onOptionSelected">Callback when an option is selected (index parameter)</param>
        /// <param name="addBackOption">Whether to include a back option at the top of the list. Index for the back option will be the count of items passed-in.</param>
        public void SetActive(int count, UnityAction<WC_OptionT, int> SetupOption, Action<int> onOptionSelected, bool addBackOption)
        {
            if (count <= 0)
            {
                Debug.LogError("WC_Lister:: Cannot show lister with zero or negative options");
                return;
            }

            // ============ Update Cache ============ //
            IsActive = true;
            _onOptionSelected = onOptionSelected;
            _hasBackOption = addBackOption;
            _totalOptions = addBackOption ? count + 1 : count;
            _selectionIndex = 0;
            _scrollOffset = 0;
            _SetupOption = SetupOption;
            int visibleCount = Mathf.Min(_totalOptions, _maxVisibleOptions);

            // ============ Update Widgets ============ //
            _overflowBottom.SetActive(false);
            _overflowTop.SetActive(false);
            SetOptionCount(visibleCount);
            UpdateOptionWidgets();
        }

        public void SetInactive()
        {
            IsActive = false;
            _overflowBottom.SetActive(false);
            _overflowTop.SetActive(false);
            // Hide all options
            foreach (var option in _optionControllers)
            {
                option.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Input Handling
        // ----------------------------------------
        // Input Handling 
        // ----------------------------------------

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
                Debug.LogError("Input_Cancel called while lister is not active");
                return;
            }
            if (_hasBackOption)
            {
                _selectionIndex = _totalOptions - 1; // Select back option
                Input_Confirm(); // Execute back action
            }
        }

        public void Input_Confirm()
        {
            if (!IsActive)
            {
                Debug.LogError("Input_Confirm called while lister is not active");
                return;
            }
            _onOptionSelected?.Invoke(_selectionIndex);
        }

        public void Input_Down()
        {
            if (!IsActive)
            {
                Debug.LogError("Input_Down called while lister is not active");
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
            UpdateOptionWidgets();
        }

        public void Input_Up()
        {
            if (!IsActive)
            {
                Debug.LogError("Input_Up called while lister is not active");
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
            UpdateOptionWidgets();
        }
        #endregion

        #region Private API
        // ----------------------------------------
        // Private API 
        // ----------------------------------------

        /// <param name="optionIndex">Is the index of the option not the widget</param>
        private void SetSelected(int optionIndex)
        {
            int oldWidgetIndex = _selectionIndex - _scrollOffset;

            if (oldWidgetIndex >= 0 && oldWidgetIndex < _optionControllers.Count)
            {
                _optionControllers[oldWidgetIndex].SetIsHighlighted(false);
            }

            _selectionIndex = optionIndex;

            // Add highlighting to new selection
            int newWidgetIndex = _selectionIndex - _scrollOffset;
            if (newWidgetIndex >= 0 && newWidgetIndex < _optionControllers.Count)
            {
                _optionControllers[newWidgetIndex].SetIsHighlighted(true);
            }
        }

        private void UpdateOptionWidgets()
        {
            // Update each visible option controller with the correct data
            for (int i = 0; i < _optionControllers.Count; i++)
            {
                _optionControllers[i].SetIsHighlighted(false);

                int dataIndex = i + _scrollOffset;
                _SetupOption(_optionControllers[i], dataIndex);
                _optionControllers[i].gameObject.SetActive(true);
            }

            // Update overflow arrows
            UpdateOverflowArrows();

            // Update selection highlighting
            int visualIndex = _selectionIndex - _scrollOffset;
            if (visualIndex >= 0 && visualIndex < _optionControllers.Count)
            {
                _optionControllers[visualIndex].SetIsHighlighted(true);
            }
        }

        private void UpdateOverflowArrows()
        {
            bool hasOptionsAbove = _scrollOffset > 0;
            bool hasOptionsBelow = _scrollOffset + _optionControllers.Count < _totalOptions;

            _overflowTop.SetActive(hasOptionsAbove);
            _overflowBottom.SetActive(hasOptionsBelow);
        }

        private void SetOptionCount(int count)
        {
            // Remove excess options
            if (_optionControllers.Count > count)
            {
                for (int i = _optionControllers.Count - 1; i >= count; i--)
                {
                    GameObject.Destroy(_optionControllers[i].gameObject);
                }
                _optionControllers.RemoveRange(count, _optionControllers.Count - count);
            }
            // Add missing options
            else if (_optionControllers.Count < count)
            {
                for (int i = _optionControllers.Count; i < count; i++)
                {
                    WC_OptionT option = GameObject.Instantiate(_optionPrefab, _optionParent);
                    option.gameObject.SetActive(true);
                    _optionControllers.Add(option);
                }
            }
        }
        #endregion
    }
}
