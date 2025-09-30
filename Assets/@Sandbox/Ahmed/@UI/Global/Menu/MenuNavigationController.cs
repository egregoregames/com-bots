using System.Collections.Generic;
using System.Linq;
using ComBots.Global.UI.Menu;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.UI.Controllers;
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

        [Header("Panel Controllers")]
        [SerializeField] private PlannerController _plannerController;
        [SerializeField] private BotlinkController _botlinkController;
        [SerializeField] private BackpackController _backpackController;
        [SerializeField] private MapController _mapController;
        [SerializeField] private SocialyteController _socialyteController;
        [SerializeField] private SettingsController _settingsController;
        [SerializeField] private SaveController _saveController;

        // Description elements
        private VisualElement _VE_root;
        private Label _VE_descriptionLabel;
        private Label _VE_headerLabel;
        private VisualElement _VE_description;

        private List<VisualElement> _VE_navItems;
        private int _selectedIndex = 0;
        
        // Panel controllers array for easy access by index
        private UIController[] _panelControllers;


        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;

            // Navigation items
            _VE_navItems = _VE_root.Query<VisualElement>(className: CLASS_NAV_ITEM).ToList();
            // Description elements
            _VE_descriptionLabel = _VE_root.Q<Label>(className: CLASS_NAV_DESCRIPTION_LABEL);
            _VE_headerLabel = _VE_root.Q<Label>(className: CLASS_NAV_DESCRIPTION_HEADER);
            _VE_description = _VE_root.Q<VisualElement>(className: CLASS_NAV_DESCRIPTION);
            
            // Initialize panel controllers array in the same order as navigation items
            // Based on UXML: Planner, Botlink, Backpack, Map, Socialyte, Settings, Save
            _panelControllers = new UIController[]
            {
                _plannerController,
                _botlinkController, 
                _backpackController,
                _mapController,
                _socialyteController,
                _settingsController,
                _saveController
            };
            foreach (var controller in _panelControllers)
            {
                controller.TryInit();
            }
            
            // Log panel controller references for debugging
            ValidateControllerReferences();
            
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
            _panelControllers = null;
        }

        public void SetActive(bool isActive)
        {
            if (_VE_navItems == null || _VE_navItems.Count == 0) { return; }
            if (_selectedIndex >= 0 && _selectedIndex < _VE_navItems.Count)
            {
                if (isActive)
                {
                    _VE_navItems[_selectedIndex].AddToClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
                    // Activate initial panel
                    if (_selectedIndex >= 0 && _selectedIndex < _panelControllers.Length && _panelControllers[_selectedIndex] != null)
                    {
                        ActivatePanel(_selectedIndex);
                    }
                }
                else
                {
                    _VE_navItems[_selectedIndex].RemoveFromClassList(CLASS_NAV_ITEM_HIGHLIGHTED);
                    // Deactivate current panel
                    if (_selectedIndex >= 0 && _selectedIndex < _panelControllers.Length && _panelControllers[_selectedIndex] != null)
                    {
                        DeactivatePanel(_selectedIndex);
                    }
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

            // Deactivate current panel if valid
            if (_selectedIndex >= 0 && _selectedIndex < _panelControllers.Length && _panelControllers[_selectedIndex] != null)
            {
                DeactivatePanel(_selectedIndex);
            }

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
            
            // Activate new panel if valid
            if (_selectedIndex >= 0 && _selectedIndex < _panelControllers.Length && _panelControllers[_selectedIndex] != null)
            {
                ActivatePanel(_selectedIndex);
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

        private void ValidateControllerReferences()
        {
            MyLogger<MenuNavigationController>.StaticLog("=== Validating Panel Controller References ===");
            string[] panelNames = { "Planner", "Botlink", "Backpack", "Map", "Socialyte", "Settings", "Save" };
            
            for (int i = 0; i < _panelControllers.Length; i++)
            {
                if (_panelControllers[i] == null)
                {
                    MyLogger<MenuNavigationController>.StaticLog($"ERROR: Panel Controller #{i} ({panelNames[i]}) is NULL!");
                }
                else
                {
                    MyLogger<MenuNavigationController>.StaticLog($"Panel Controller #{i} ({panelNames[i]}) is valid: {_panelControllers[i].GetType().Name}");
                }
            }
            MyLogger<MenuNavigationController>.StaticLog("=== End Validation ===");
        }

        private void ActivatePanel(int index)
        {
            if (index < 0 || index >= _panelControllers.Length)
            {
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Cannot activate panel - invalid index {index}");
                return;
            }

            var controller = _panelControllers[index];
            if (controller == null)
            {
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Cannot activate panel #{index} - controller is null");
                return;
            }

            MyLogger<MenuNavigationController>.StaticLog($"Activating panel #{index}: {controller.GetType().Name}");
            
            if (controller is PlannerController planner)
                planner.SetActive();
            else if (controller is BotlinkController botlink)
                botlink.SetActive();
            else if (controller is BackpackController backpack)
                backpack.SetActive();
            else if (controller is MapController map)
                map.SetActive();
            else if (controller is SocialyteController socialyte)
                socialyte.SetActive();
            else if (controller is SettingsController settings)
                settings.SetActive();
            else if (controller is SaveController save)
                save.SetActive();
            else
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Unknown controller type: {controller.GetType().Name}");
        }

        private void DeactivatePanel(int index)
        {
            if (index < 0 || index >= _panelControllers.Length)
            {
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Cannot deactivate panel - invalid index {index}");
                return;
            }

            var controller = _panelControllers[index];
            if (controller == null)
            {
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Cannot deactivate panel #{index} - controller is null");
                return;
            }

            MyLogger<MenuNavigationController>.StaticLog($"Deactivating panel #{index}: {controller.GetType().Name}");
            
            if (controller is PlannerController planner)
                planner.SetInactive();
            else if (controller is BotlinkController botlink)
                botlink.SetInactive();
            else if (controller is BackpackController backpack)
                backpack.SetInactive();
            else if (controller is MapController map)
                map.SetInactive();
            else if (controller is SocialyteController socialyte)
                socialyte.SetInactive();
            else if (controller is SettingsController settings)
                settings.SetInactive();
            else if (controller is SaveController save)
                save.SetInactive();
            else
                MyLogger<MenuNavigationController>.StaticLog($"ERROR: Unknown controller type: {controller.GetType().Name}");
        }
    }
}