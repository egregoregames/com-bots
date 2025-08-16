using System.Collections;
using UnityEngine;

namespace ComBots.Utils.EntryPoints
{
    /// <summary>
    /// Base class for entry points.
    /// </summary>
    public interface IAsyncEntryPoint
    {
        /// <summary>
        /// Initializes the entry point.
        /// </summary>
        public IEnumerator Async_TryInit();

        /// <summary>
        /// Cleans up the entry point.
        /// </summary>
        public void Dispose();
    }
}