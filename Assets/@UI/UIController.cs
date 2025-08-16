using ComBots.Utils.EntryPoints;
using UnityEngine;

namespace ComBots.UI.Controllers
{
    public abstract class UIController : EntryPointMono
    {
        /// <summary>
        /// The name of the user interface this controller manages.
        /// This should be overridden in derived classes to provide the specific UI name.
        /// Example: "Game.Menu", "Game.Inventory", etc.
        /// </summary>
        protected abstract string UserInterfaceName { get; }
        protected virtual void OnValidate()
        {
            SetupGameObjectName();
        }

        protected virtual void Start()
        {
            SetupGameObjectName();
        }

        private void SetupGameObjectName()
        {
            gameObject.name = $"[UI.Controller] {UserInterfaceName}";
        }
    }
}