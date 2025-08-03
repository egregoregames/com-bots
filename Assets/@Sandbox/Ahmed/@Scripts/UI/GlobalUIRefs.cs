using ComBots.Global.UI.Dialogue;
using ComBots.Utils.EntryPoints;
using UnityEngine;

namespace ComBots.Global.UI
{
    public class GlobalUIRefs : EntryPointMono
    {
        public static GlobalUIRefs I{ get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [Header("Menu")]
        [SerializeField] private Menu.MenuController _menuController;
        public Menu.MenuController MenuController => _menuController;
        [SerializeField] private Menu.MenuNavigationController _menuNavigationController;

        [Header("Dialogue")]
        [SerializeField] private DialogueController _dialogueController;
        public DialogueController DialogueController => _dialogueController;

        [Header("Area Transitions")]
        [SerializeField] private StageDoorTransitions _stageDoorTransitions;
        public StageDoorTransitions StageDoorTransitions => _stageDoorTransitions;

        protected override void Init()
        {
            //Independent
            _menuController.TryInit();
            _menuNavigationController.TryInit();
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