using System.Collections.Generic;
using ComBots.Logs;
using UnityEngine;

namespace ComBots.Utils.ObjectPooling
{
    public class GameObjectPool : MonoBehaviour, IPool
    {
        [Header("Pool Info")]
        [SerializeField] private string _key;
        public string Key => _key;
        [Header("Prefab")]
        [SerializeField] private Transform _parentT;
        [SerializeField] private GameObject _prefab;
        [Header("Limits")]
        [SerializeField, Min(0)] private int _initialSize = 0;
        [SerializeField, Min(1)] private int _maxSize = 1;
        // Stacks
        private Stack<GameObject> _available;

        void Start()
        {
            _available = new();
            for (int i = 0; i < _initialSize; i++)
            {
                var obj = Instantiate();
                Push(obj);
            }
            PoolManager.I.RegisterPool(this);
        }

        public T Pull<T>() where T : MonoBehaviour
        {
            GameObject obj;
            if (_available.Count == 0)
            {
                obj = Instantiate();
            }
            else
            {
                obj = _available.Pop();
            }
            obj.SetActive(true);
            return obj.GetComponent<T>();
        }

        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            if (_available.Count < _maxSize)
            {
                _available.Push(obj);
            }
            else
            {
                Destroy(obj);
            }
        }

        private GameObject Instantiate()
        {
            var obj = Instantiate(_prefab, _parentT);
            return obj;
        }
    }
}