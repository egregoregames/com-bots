using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class SaveController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Save";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_savePanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_savePanel = _VE_root.Q<VisualElement>("Save");
            
            MyLogger<SaveController>.StaticLog($"SaveController initialized. Panel found: {_VE_savePanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_savePanel = null;
        }

        public void SetActive()
        {
            MyLogger<SaveController>.StaticLog("SetActive() called");
            if (_VE_savePanel != null)
            {
                _VE_savePanel.style.display = DisplayStyle.Flex;
                MyLogger<SaveController>.StaticLog("Save panel activated (display: flex)");
            }
            else
            {
                MyLogger<SaveController>.StaticLog("ERROR: Cannot activate - _VE_savePanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<SaveController>.StaticLog("SetInactive() called");
            if (_VE_savePanel != null)
            {
                _VE_savePanel.style.display = DisplayStyle.None;
                MyLogger<SaveController>.StaticLog("Save panel deactivated (display: none)");
            }
            else
            {
                MyLogger<SaveController>.StaticLog("ERROR: Cannot deactivate - _VE_savePanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle save-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
