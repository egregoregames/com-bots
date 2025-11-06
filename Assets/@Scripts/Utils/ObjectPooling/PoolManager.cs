using System.Collections.Generic;
using ComBots.Logs;
using UnityEngine;

namespace ComBots.Utils.ObjectPooling
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager I { get; private set; }

        private Dictionary<string, IPool> _pools;
        
        void Awake()
        {
            if (I == null)
            {
                I = this;
                _pools = new();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterPool(IPool pool)
        {
            _pools.Add(pool.Key, pool);
        }

        public T Pull<T>(string key) where T : Component
        {
            if (_pools.TryGetValue(key, out var pool))
            {
                return pool.Pull<T>();
            }
            MyLogger<PoolManager>.StaticLogError($"Pull(): Pool with key {key} not found.");
            return default;
        }

        public void Push<T>(string key, T obj) where T : Component {
            if (_pools.TryGetValue(key, out var pool))
            {
                pool.Push(obj.gameObject);
            }
            else
            {
                MyLogger<PoolManager>.StaticLogError($"Push(): Pool with key {key} not found.");
            }
        }

        void OnDestroy()
        {
            if (I == this)
            {
                I = null;
            }
            _pools = null;
        }
    }
}