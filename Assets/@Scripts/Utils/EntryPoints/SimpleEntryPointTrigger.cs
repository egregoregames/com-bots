using System.Collections;
using ComBots.Utils.EntryPoints;
using ComBots.Logs;
using UnityEngine;

namespace ComBots.Game.UI.Menu
{
    public class SimpleEntryPointTrigger : AsyncEntryPointMono
    {
        [SerializeField] private GameObject[] entryPoints;

        private IEnumerator Start()
        {
            yield return Async_TryInit();
        }

        protected override IEnumerator Async_Init()
        {
            MyLogger<SimpleEntryPointTrigger>.StaticLog($"<{gameObject.name}> Initializing {entryPoints.Length} entry points.");
            foreach (var entryPoint in entryPoints)
            {
                if (entryPoint.TryGetComponent(out IEntryPoint entry))
                {
                    entry.TryInit();
                }
                else if (entryPoint.TryGetComponent(out IAsyncEntryPoint asyncEntry))
                {
                    yield return asyncEntry.Async_TryInit();
                }
            }
        }

        void OnValidate()
        {
            if (entryPoints == null || entryPoints.Length == 0)
            {
                return;
            }

            foreach (var entryPoint in entryPoints)
            {
                if (entryPoint == null)
                {
                    continue;
                }
                if (entryPoint.TryGetComponent(out IEntryPoint entry))
                {
                    continue;
                }
                if (entryPoint.TryGetComponent(out IAsyncEntryPoint asyncEntry))
                {
                    continue;
                }
                MyLogger<SimpleEntryPointTrigger>.StaticLogError($"Entry point {entryPoint.name} does not implement IEntryPoint or IAsyncEntryPoint.");
                break;
            }
        }

        public override void Dispose()
        {
        }
    }
}