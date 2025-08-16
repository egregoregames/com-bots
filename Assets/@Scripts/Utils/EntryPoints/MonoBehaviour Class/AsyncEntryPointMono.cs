using System.Collections;
using UnityEngine;

namespace ComBots.Utils.EntryPoints
{
    /// <summary>
    /// Base class for entry points.
    /// </summary>
    public abstract class AsyncEntryPointMono : MonoBehaviour, IAsyncEntryPoint
    {
        protected bool _isEntryPointInitialized = false;

        public abstract void Dispose();

        public IEnumerator Async_TryInit()
        {
            if (_isEntryPointInitialized) { yield break; }

            yield return Async_Init();

            _isEntryPointInitialized = true;
        }

        protected abstract IEnumerator Async_Init();
    }
}