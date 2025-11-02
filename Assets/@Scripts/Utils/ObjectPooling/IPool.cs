using UnityEngine;

namespace ComBots.Utils.ObjectPooling
{
    public interface IPool
    {
        public string Key{ get; }
        public T Pull<T>() where T : Component;
        public void Push(GameObject obj);
    }
}