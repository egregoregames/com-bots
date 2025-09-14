using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class SettingsController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Settings";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_settingsPanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_settingsPanel = _VE_root.Q<VisualElement>("Settings");
            
            MyLogger<SettingsController>.StaticLog($"SettingsController initialized. Panel found: {_VE_settingsPanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_settingsPanel = null;
        }

        public void SetActive()
        {
            MyLogger<SettingsController>.StaticLog("SetActive() called");
            if (_VE_settingsPanel != null)
            {
                _VE_settingsPanel.style.display = DisplayStyle.Flex;
                MyLogger<SettingsController>.StaticLog("Settings panel activated (display: flex)");
            }
            else
            {
                MyLogger<SettingsController>.StaticLog("ERROR: Cannot activate - _VE_settingsPanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<SettingsController>.StaticLog("SetInactive() called");
            if (_VE_settingsPanel != null)
            {
                _VE_settingsPanel.style.display = DisplayStyle.None;
                MyLogger<SettingsController>.StaticLog("Settings panel deactivated (display: none)");
            }
            else
            {
                MyLogger<SettingsController>.StaticLog("ERROR: Cannot deactivate - _VE_settingsPanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle settings-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
