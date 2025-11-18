public class MonoProtectedSingletonR3<T> : MonoBehaviourR3
{
    protected static T Instance { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = GetComponent<T>();
    }
}