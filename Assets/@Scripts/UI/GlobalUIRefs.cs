using ComBots.Utils.EntryPoints;
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

        [Header("Area Transitions")]
        [SerializeField] private StageDoorTransitions _stageDoorTransitions;
        public StageDoorTransitions StageDoorTransitions => _stageDoorTransitions;

        protected override void Init()
        {
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