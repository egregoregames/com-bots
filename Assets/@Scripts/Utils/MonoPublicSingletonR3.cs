using UnityEngine;

public class MonoPublicSingletonR3<T> : MonoBehaviourR3 where T : Component
{
    public static T Instance { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = GetComponent<T>();
    }
}