using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class SocialyteController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Socialyte";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_socialytePanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_socialytePanel = _VE_root.Q<VisualElement>("Socialyte");
            
            MyLogger<SocialyteController>.StaticLog($"SocialyteController initialized. Panel found: {_VE_socialytePanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_socialytePanel = null;
        }

        public void SetActive()
        {
            MyLogger<SocialyteController>.StaticLog("SetActive() called");
            if (_VE_socialytePanel != null)
            {
                _VE_socialytePanel.style.display = DisplayStyle.Flex;
                MyLogger<SocialyteController>.StaticLog("Socialyte panel activated (display: flex)");
            }
            else
            {
                MyLogger<SocialyteController>.StaticLog("ERROR: Cannot activate - _VE_socialytePanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<SocialyteController>.StaticLog("SetInactive() called");
            if (_VE_socialytePanel != null)
            {
                _VE_socialytePanel.style.display = DisplayStyle.None;
                MyLogger<SocialyteController>.StaticLog("Socialyte panel deactivated (display: none)");
            }
            else
            {
                MyLogger<SocialyteController>.StaticLog("ERROR: Cannot deactivate - _VE_socialytePanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle socialyte-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
