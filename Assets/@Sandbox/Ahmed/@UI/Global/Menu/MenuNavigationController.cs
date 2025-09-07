using System.Collections.Generic;
using System.Linq;
using ComBots.Global.UI.Menu;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class MenuNavigationController : EntryPointMono, IInputHandler
    {
        private const string CLASS_NAV_ITEM = "nav-item";
        private const string CLASS_NAV_ITEM_HIGHLIGHTED = "nav-item-highlighted";
        private const string CLASS_NAV_DESCRIPTION_LABEL = "nav-description-label";
        private const string CLASS_NAV_DESCRIPTION_HEADER = "nav-description-title";
        private const string CLASS_NAV_DESCRIPTION = "nav-description";

        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;

        [Header("Navigation")]
        [SerializeField] private NavigationItemConfig[] _navigationItemConfigs;

        // Description elements
        private VisualElement _VE_root;
        private Label _VE_descriptionLabel;
        private Label _VE_headerLabel;
        private VisualElement _VE_description;

        private List<VisualElement> _VE_navItems;
        private int _selectedIndex = 0;


        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;

            // Navigation items
            _VE_navItems = _VE_root.Query<VisualElement>(className: CLASS_NAV_ITEM).ToList();
            // Description elements
            _VE_descriptionLabel = _VE_root.Q<Label>(className: CLASS_NAV_DESCRIPTION_LABEL);
            _VE_headerLabel = _VE_root.Q<Label>(className: CLASS_NAV_DESCRIPTION_HEADER);
            _VE_description = _VE_root.Q<VisualElement>(className: CLASS_NAV_DESCRIPTION);
            // Set initial selection
            _selectedIndex = 0;
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_navItems = null;
            _VE_descriptionLabel = null;
            _VE_headerLabel = null;
            _VE_description = null;
        }

        public void SetActive(bool isActive)
        {
            if (_VE_navItems == null || _VE_navItems.Count == 0) { return; }
            if (_selectedIndex >= 0 && _selectedIndex < _VE_navItems.Count)
            {
                if (isActive)
                {
                    _VE_navItems[_selectedIndex].AddToClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
                }
                else
                {
                    _VE_navItems[_selectedIndex].RemoveFromClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
                }
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            if (_VE_navItems == null || _VE_navItems.Count == 0) return false;
            MyLogger<MenuNavigationController>.StaticLog($"HandleInput({actionName})");
            switch (actionName)
            {
                case "navigate":
                    if (context.performed)
                    {
                        var direction = context.ReadValue<Vector2>();
                        if (direction.x > 0f)
                        {
                            Input_Right();
                        }
                        else if (direction.x < 0f)
                        {
                            Input_Left();
                        }
                    }
                    return true;
            }
            return false;
        }

        private void Input_Right()
        {
            int newIndex = _selectedIndex + 1;
            if (newIndex >= _VE_navItems.Count)
            {
                newIndex = 0; // Wrap around to first item
            }
            SetSelected(newIndex);
        }

        private void Input_Left()
        {
            int newIndex = _selectedIndex - 1;
            if (newIndex < 0)
            {
                newIndex = _VE_navItems.Count - 1; // Wrap around to last item
            }
            SetSelected(newIndex);
        }

        private void SetSelected(int index)
        {
            if (index < 0 || index >= _VE_navItems.Count) return;

            // Remove highlighting from current selection
            if (_selectedIndex >= 0 && _selectedIndex < _VE_navItems.Count)
            {
                _VE_navItems[_selectedIndex].RemoveFromClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
            }

            // Set new selection
            _selectedIndex = index;
            // Add highlighting to new selection
            _VE_navItems[_selectedIndex].AddToClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
            // Fill out description
            var config = _navigationItemConfigs[_selectedIndex];
            _VE_descriptionLabel.text = config.description;
            _VE_headerLabel.text = config.header;
            // Change description background
            _VE_description.style.backgroundImage = new StyleBackground(config.background);
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