public class MonoPublicSingletonR3<T> : MonoBehaviourR3
{
    public static T Instance { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();
        Instance = GetComponent<T>();
    }
}