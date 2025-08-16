using UnityEngine;

namespace ComBots.Utils.EntryPoints
{
    /// <summary>
    /// Base class for entry points.
    /// </summary>
    public abstract class EntryPointMono : MonoBehaviour, IEntryPoint
    {
        protected bool _isEntryPointInitialized = false;

        public abstract Dependency Dependency { get; }

        public void TryInit()
        {
            if (_isEntryPointInitialized) { return; }

            Init();

            _isEntryPointInitialized = true;
        }

        protected abstract void Init();

        public abstract void Dispose();
    }
}