using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Inherited class for all Pause Menu apps, such as the Planner, Socialyte,
/// Backpack, etc
/// </summary>
/// <typeparam name="T">The singleton logic class for the app</typeparam>
public class PauseMenuAppSingleton<T> : MonoProtectedSingletonR3<T> where T : Component
{
    private static PauseMenuAppSingleton<T> _instance;

    [field: SerializeField, ReadOnly]
    protected bool RefreshInProgress { get; set; }

    private PauseMenuApp _pauseMenuApp;

    public static void Open()
    {
        Instance.gameObject.SetActive(true);
        _instance.EnsurePauseMenuApp();
        _instance._pauseMenuApp.PlaySoundMenuOpened();
    }

    protected new virtual void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    protected override void Initialize()
    {
        base.Initialize();
        _instance = this;
        RefreshInProgress = false;
    }

    private void EnsurePauseMenuApp()
    {
        if (_pauseMenuApp == null)
        {
            _pauseMenuApp = GetComponent<PauseMenuApp>();
        }
    }

    protected void PlaySoundNavigation()
    {
        EnsurePauseMenuApp();
        _pauseMenuApp.PlaySoundNavigation();
    }

    protected void PlaySoundSubmit()
    {
        EnsurePauseMenuApp();
        _pauseMenuApp.PlaySoundSubmit();
    }
}