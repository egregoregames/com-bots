using UnityEngine;

public class MonoProtectedSingletonR3<T> : MonoBehaviourR3 where T : Component
{
    protected static T Instance { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = GetComponent<T>();
    }
}