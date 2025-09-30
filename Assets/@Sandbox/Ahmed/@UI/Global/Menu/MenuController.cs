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

namespace ComBots.Sandbox.Global.UI.Menu
{
    public class MenuController : UIController, IInputHandler
    {
        protected override string UserInterfaceName => "Game.Menu";

        private const string NAME_GLOBAL = "Global";
        private const string CLASS_BOTTOM = "bottom";
        private const string CLASS_BOTTOM_HIDDEN = "bottom-hidden";
        private const string CLASS_BOTTOM_PEAKING = "bottom-peaking";

        public override Dependency Dependency => Dependency.Independent;

        [Header("UI")]
        [SerializeField] private UIDocument uiDocument;
        private VisualElement _VE_root;
        private VisualElement _VE_Bottom;
        private const string CLASS_ROOT_HIDDEN = "menu-hidden";

        [SerializeField] MenuNavigationController _navigationController;
        private bool _isMenuOpen;

        protected override void Init()
        {
            _VE_root = uiDocument.rootVisualElement.Q<VisualElement>(name = NAME_GLOBAL);
            _VE_Bottom = _VE_root.Q<VisualElement>(className: CLASS_BOTTOM);
        }

        public override void Dispose()
        {
            _VE_root = null;
        }

        public void SetActive(bool isActive)
        {
            MyLogger<MenuController>.StaticLog($"SetActive({isActive})");
            _VE_root.EnableInClassList(CLASS_ROOT_HIDDEN, !isActive);
            SetBottomState(isActive ? BottomState.Visible : BottomState.Peaking);
            _isMenuOpen = isActive;
            _navigationController.SetActive(isActive);
            //GlobalConfig.I.UISo.OnCameraTransition?.Invoke(_isMenuOpen);
        }

        public void SetBottomBarVisible(bool isVisible)
        {
            if (isVisible)
            {
                SetBottomState(_isMenuOpen ? BottomState.Visible : BottomState.Peaking);
            }
            else
            {
                SetBottomState(BottomState.Hidden);
            }
        }

        private enum BottomState
        {
            Peaking,
            Hidden,
            Visible
        }
        private void SetBottomState(BottomState state)
        {
            switch (state)
            {
                case BottomState.Peaking:
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_PEAKING, true);
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_HIDDEN, false);
                    break;
                case BottomState.Hidden:
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_PEAKING, false);
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_HIDDEN, true);
                    break;
                case BottomState.Visible:
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_PEAKING, false);
                    _VE_Bottom.EnableInClassList(CLASS_BOTTOM_HIDDEN, false);
                    break;
            }
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