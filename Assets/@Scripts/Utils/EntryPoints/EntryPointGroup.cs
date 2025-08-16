using UnityEngine;

namespace ComBots.Utils.EntryPoints
{
    public class EntryPointGroup : MonoBehaviour, IEntryPoint
    {
        public IEntryPoint[] EntryPoints;

        public Dependency Dependency => Dependency.Independent;

        public void Dispose()
        {
            foreach (IEntryPoint entryPoint in EntryPoints)
            {
                if (entryPoint == null) continue;

                entryPoint.Dispose();
            }
        }

        public void TryInit()
        {
            foreach(IEntryPoint entryPoint in EntryPoints)
            {
                if (entryPoint == null) continue;

                entryPoint.TryInit();
            }
        }

        void OnDestroy()
        {
            Dispose();
        }
    }
}