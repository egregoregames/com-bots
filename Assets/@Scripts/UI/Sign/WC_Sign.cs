using ComBots.Game.StateMachine;
using ComBots.Inputs;
using ComBots.Signs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Sign
{
    public class WC_Sign : UIController
    {
        // =============== UIController Implementation =============== //
        public override Dependency Dependency => Dependency.Independent;
        protected override string UserInterfaceName => "Sign";
        // =============== UI =============== //
        [SerializeField] private GameObject widget;
        [SerializeField] private TextMeshProUGUI text;

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        public void SetActive(string signText)
        {
            text.text = signText;
            widget.SetActive(true);
        }

        public void SetInactive()
        {
            widget.SetActive(false);
        }

        #endregion

        #region UIController Implementation
        // ----------------------------------------
        // UIController Implementation
        // ----------------------------------------

        public override void Dispose()
        {

        }

        protected override void Init()
        {

        }

        #endregion

        #region Input Management
        // ----------------------------------------
        // Input Management 
        // ----------------------------------------

        public void Input_Dismiss()
        {
            GameStateMachine.I.ExitState<GameStateMachine.State_Sign>();
        }

        #endregion
    }
}
