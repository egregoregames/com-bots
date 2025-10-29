using ComBots.Battles;
using ComBots.Inputs;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ComBots.Global.UI.ItemReceival
{
    public class WC_ItemReceival : UIController, IInputHandler
    {

        // =============== UIController Implementation =============== //
        protected override string UserInterfaceName { get; } = "Item-Receival";
        public override Dependency Dependency => Dependency.Independent;
        // =============== Widget =============== //
        [Header("Widget")]
        [SerializeField] private GameObject _widget;
        // =============== Content =============== //
        [Header("Content")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _okButton;
        [SerializeField] private Image _icon;
        private UnityAction _completionCallback;
        // =============== Active Status =============== //
        private bool _isActive = false;

        #region Unity Lifecycle
        // ----------------------------------------
        // Unity Lifecycle 
        // ----------------------------------------

        void Awake()
        {
            _okButton.onClick.AddListener(UI_Button_Ok);
        }

        void Update()
        {
            if (!_isActive)
            {
                return;
            }
        }

        #endregion

        #region Public API
        // ----------------------------------------
        // Public API 
        // ----------------------------------------

        /// <summary>
        /// Opens the item receival widget and shows the provided software info.
        /// </summary>
        /// <param name="completionCallback">The callback to invoke when the user confirms the item receival. You must call SetInactive() manually for this controller</param>
        /// <param name="software"></param>
        public void SetActive(UnityAction completionCallback, Software software)
        {
            _isActive = true;
            _widget.SetActive(true);
            _completionCallback = completionCallback;
        }

        /// <summary>
        /// Closes the item receival widget.
        /// </summary>
        public void SetInactive()
        {
            _isActive = false;
            _widget.SetActive(false);
            _completionCallback = null;
        }

        #endregion

        #region Private Methods
        // ----------------------------------------
        // Private Methods 
        // ----------------------------------------

        private void UI_Button_Ok()
        {
            _completionCallback?.Invoke();
        }

        #endregion

        #region UIController Implementation
        // ----------------------------------------
        // UIController Implementation 
        // ----------------------------------------

        protected override void Init()
        {
        }

        public override void Dispose()
        {
        }

        #endregion

        #region IInputHandler Interface
        // ----------------------------------------
        // IInputHandler Interface 
        // ----------------------------------------

        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case "Submit":
                    if (context.performed && _isActive)
                    {
                        UI_Button_Ok();
                        return true;
                    }
                    break;
            }
            return false;
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

        #endregion
    }
}