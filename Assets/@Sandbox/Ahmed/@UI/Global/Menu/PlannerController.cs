using ComBots.Logs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class PlannerController : UIController
    {
        protected override string UserInterfaceName => "Global.Menu.Planner";
        
        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument _uiDocument;
        
        private VisualElement _VE_root;
        private VisualElement _VE_plannerPanel;
        
        protected override void Init()
        {
            _VE_root = _uiDocument.rootVisualElement;
            _VE_plannerPanel = _VE_root.Q<VisualElement>("Planner");
            
            MyLogger<PlannerController>.StaticLog($"PlannerController initialized. Panel found: {_VE_plannerPanel != null}");
        }

        public override void Dispose()
        {
            _VE_root = null;
            _VE_plannerPanel = null;
        }

        public void SetActive()
        {
            MyLogger<PlannerController>.StaticLog("SetActive() called");
            if (_VE_plannerPanel != null)
            {
                _VE_plannerPanel.style.display = DisplayStyle.Flex;
                MyLogger<PlannerController>.StaticLog("Planner panel activated (display: flex)");
            }
            else
            {
                MyLogger<PlannerController>.StaticLog("ERROR: Cannot activate - _VE_plannerPanel is null");
            }
        }

        public void SetInactive()
        {
            MyLogger<PlannerController>.StaticLog("SetInactive() called");
            if (_VE_plannerPanel != null)
            {
                _VE_plannerPanel.style.display = DisplayStyle.None;
                MyLogger<PlannerController>.StaticLog("Planner panel deactivated (display: none)");
            }
            else
            {
                MyLogger<PlannerController>.StaticLog("ERROR: Cannot deactivate - _VE_plannerPanel is null");
            }
        }

        public bool HandleInput(InputAction.CallbackContext context, string actionName)
        {
            // Handle planner-specific input here
            // For now, just return false to indicate no input was handled
            return false;
        }
    }
}
