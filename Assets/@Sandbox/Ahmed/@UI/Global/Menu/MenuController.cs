using System;
using System.Collections.Generic;
using ComBots.Game;
using ComBots.Game.StateMachine;
using ComBots.Game.Worlds.Rooms;
using ComBots.Inputs;
using ComBots.Logs;
using ComBots.src;
using ComBots.UI.Controllers;
using ComBots.Utils.EntryPoints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace ComBots.Global.UI.Menu
{
    public class MenuController : UIController, IInputHandler
    {
        protected override string UserInterfaceName => "Game.Menu";

        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument uiDocument;
        private VisualElement VE_root;
        private const string CLASS_ROOT_HIDDEN = "menu-hidden";

        [SerializeField] MenuNavigationController _navigationController;
        private bool _isMenuOpen;

        protected override void Init()
        {
            VE_root = uiDocument.rootVisualElement.Q<VisualElement>(name = "Global");
        }

        public override void Dispose()
        {
            VE_root = null;
        }

        public void SetActive(bool isActive)
        {
            MyLogger<MenuController>.StaticLog($"SetActive({isActive})");
            if (isActive)
            {
                VE_root.EnableInClassList(CLASS_ROOT_HIDDEN, false);
            }
            else
            {
                VE_root.EnableInClassList(CLASS_ROOT_HIDDEN, true);
            }
            _isMenuOpen = isActive;
            _navigationController.SetActive(isActive);
            //GlobalConfig.I.UISo.OnCameraTransition?.Invoke(_isMenuOpen);
        }

        private void Input_Pause()
        {
            GameStateMachine.I.SetState<GameStateMachine.State_Playing>(null);
        }

        #region IInputHandler Implementation
        public bool HandleInput(InputAction.CallbackContext context, string actionName, InputFlags inputFlag)
        {
            switch (actionName)
            {
                case "pause":
                    if (context.performed)
                    {
                        Input_Pause();
                    }
                    return true;
            }
            return false;
        }

        public void OnInputContextPaused(InputContext context)
        {
        }

        public void OnInputContextResumed(InputContext context)
        {
        }

        public void OnInputContextEntered(InputContext context)
        {
        }

        public void OnInputContextExited(InputContext context)
        {
        }
        #endregion
    }
}