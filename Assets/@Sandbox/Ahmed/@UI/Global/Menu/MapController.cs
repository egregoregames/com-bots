using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class MapController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Map";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_mapPanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_mapPanel = _VE_root.Q<VisualElement>("Map");
            
            MyLogger<MapController>.StaticLog($"MapController initialized. Panel found: {_VE_mapPanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_mapPanel = null;
        }

        public void SetActive()
        {
            MyLogger<MapController>.StaticLog("SetActive() called");
            if (_VE_mapPanel != null)
            {
                _VE_mapPanel.style.display = DisplayStyle.Flex;
                MyLogger<MapController>.StaticLog("Map panel activated (display: flex)");
            }
            else
            {
                MyLogger<MapController>.StaticLog("ERROR: Cannot activate - _VE_mapPanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<MapController>.StaticLog("SetInactive() called");
            if (_VE_mapPanel != null)
            {
                _VE_mapPanel.style.display = DisplayStyle.None;
                MyLogger<MapController>.StaticLog("Map panel deactivated (display: none)");
            }
            else
            {
                MyLogger<MapController>.StaticLog("ERROR: Cannot deactivate - _VE_mapPanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle map-specific input here  
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
