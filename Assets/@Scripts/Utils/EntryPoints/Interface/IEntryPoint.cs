using UnityEngine;

namespace ComBots.Utils.EntryPoints
{
    /// <summary>
    /// Base class for entry points.
    /// </summary>
    public interface IEntryPoint
    {
        /// <summary>
        /// Initializes the entry point.
        /// </summary>
        public void TryInit();

        /// <summary>
        /// Cleans up the entry point.
        /// </summary>
        public void Dispose();
        public Dependency Dependency { get; }
    }

    public enum Dependency
    {
        Dependent,
        Independent
    }
}