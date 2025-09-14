using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class BackpackController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Backpack";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_backpackPanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_backpackPanel = _VE_root.Q<VisualElement>("Backpack");
            
            MyLogger<BackpackController>.StaticLog($"BackpackController initialized. Panel found: {_VE_backpackPanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_backpackPanel = null;
        }

        public void SetActive()
        {
            MyLogger<BackpackController>.StaticLog("SetActive() called");
            if (_VE_backpackPanel != null)
            {
                _VE_backpackPanel.style.display = DisplayStyle.Flex;
                MyLogger<BackpackController>.StaticLog("Backpack panel activated (display: flex)");
            }
            else
            {
                MyLogger<BackpackController>.StaticLog("ERROR: Cannot activate - _VE_backpackPanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<BackpackController>.StaticLog("SetInactive() called");
            if (_VE_backpackPanel != null)
            {
                _VE_backpackPanel.style.display = DisplayStyle.None;
                MyLogger<BackpackController>.StaticLog("Backpack panel deactivated (display: none)");
            }
            else
            {
                MyLogger<BackpackController>.StaticLog("ERROR: Cannot deactivate - _VE_backpackPanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle backpack-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
