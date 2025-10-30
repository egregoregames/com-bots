using ComBots.Global.UI.Dialogue;
using ComBots.Utils.EntryPoints;
using UI.Sign;
using UnityEngine;

namespace ComBots.Global.UI
{
    public class GlobalUIRefs : EntryPointMono
    {
        public static GlobalUIRefs I{ get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [Header("Menu")]
        [SerializeField] private Sandbox.Global.UI.Menu.PauseMenu _menuController;
        public Sandbox.Global.UI.Menu.PauseMenu MenuController => _menuController;
        [SerializeField] private Sandbox.Global.UI.Menu.MenuNavigationController _menuNavigationController;

        [Header("Dialogue")]
        [SerializeField] private WC_Dialogue _dialogueController;
        public WC_Dialogue DialogueController => _dialogueController;

        // =============== Sign =============== //
        [Header("Sign")]
        [SerializeField] private WC_Sign _signController;
        public WC_Sign SignController => _signController;

        [Header("Area Transitions")]
        [SerializeField] private StageDoorTransitions _stageDoorTransitions;
        public StageDoorTransitions StageDoorTransitions => _stageDoorTransitions;

        protected override void Init()
        {
            //Independent
            //_menuController.TryInit();
            //_menuNavigationController.TryInit();
            _dialogueController.TryInit();
            _stageDoorTransitions.TryInit();
            I = this;
        }

        public override void Dispose()
        {
            if (I == this)
            {
                I = null;
            }
        }
    }
}