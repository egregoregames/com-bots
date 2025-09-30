using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class BotlinkController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Botlink";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_botlinkPanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_botlinkPanel = _VE_root.Q<VisualElement>("Botlink");
            
            MyLogger<BotlinkController>.StaticLog($"BotlinkController initialized. Panel found: {_VE_botlinkPanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_botlinkPanel = null;
        }

        public void SetActive()
        {
            MyLogger<BotlinkController>.StaticLog("SetActive() called");
            if (_VE_botlinkPanel != null)
            {
                _VE_botlinkPanel.style.display = DisplayStyle.Flex;
                MyLogger<BotlinkController>.StaticLog("Botlink panel activated (display: flex)");
            }
            else
            {
                MyLogger<BotlinkController>.StaticLog("ERROR: Cannot activate - _VE_botlinkPanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<BotlinkController>.StaticLog("SetInactive() called");
            if (_VE_botlinkPanel != null)
            {
                _VE_botlinkPanel.style.display = DisplayStyle.None;
                MyLogger<BotlinkController>.StaticLog("Botlink panel deactivated (display: none)");
            }
            else
            {
                MyLogger<BotlinkController>.StaticLog("ERROR: Cannot deactivate - _VE_botlinkPanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle botlink-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
