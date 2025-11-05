using ComBots.Global.UI.Dialogue;
using ComBots.Utils.EntryPoints;
using UnityEngine;

namespace ComBots.Global.UI
{
    public class GlobalUIRefs : EntryPointMono
    {
        public static GlobalUIRefs I{ get; private set; }

        public override Dependency Dependency => Dependency.Independent;

        [Header("Dialogue")]
        [SerializeField] private DialogueController _dialogueController;
        public DialogueController DialogueController => _dialogueController;

        protected override void Init()
        {
            //Independent
            //_menuController.TryInit();
            //_menuNavigationController.TryInit();
            _dialogueController.TryInit();
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