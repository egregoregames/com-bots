using System.Threading.Tasks;
using ComBots.Game.Players;
using ComBots.UI.OverheadWidgets;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Singleton that manages overhead widget pooling for UI elements that appear above NPCs and interactable objects.
/// Handles widget creation, retrieval, and return to pool for efficient reuse.
/// </summary>
public class OverheadWidgetManager : MonoBehaviourR3
{
    private static OverheadWidgetManager Instance { get; set; }

    // =============== References =============== //
    [BoxGroup("References")]
    [SerializeField] private Canvas _widgetCanvas;

    // =============== Prefabs =============== //
    [BoxGroup("Prefabs")]
    [SerializeField, Required] private WC_OverheadWidget _read_prefab;

    [BoxGroup("Prefabs")]
    [SerializeField, Required] private WC_OverheadWidget _talk_prefab;

    // =============== Pools =============== //
    [BoxGroup("Pools"), ReadOnly, ShowInInspector]
    private IObjectPool<GameObject> _read_pool;

    [BoxGroup("Pools"), ReadOnly, ShowInInspector]
    private IObjectPool<GameObject> _talk_pool;

    #region MonoBehaviourR3
    protected override void Initialize()
    {
        base.Initialize();

        if (Instance != null && Instance != this)
        {
            Log($"Multiple OverheadWidgetManager instances detected. Destroying duplicate.", LogLevel.Error);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _widgetCanvas.worldCamera = Player.I.PlayerCamera.Camera;

        // Initialize pools
        _read_pool = new ObjectPool<GameObject>(
            createFunc: () => Pool_InstantiateWidget(_read_prefab),
            actionOnGet: Pool_OnGet,
            actionOnRelease: Pool_OnRelease,
            actionOnDestroy: Pool_OnDestroy,
            collectionCheck: false,
            defaultCapacity: 2,
            maxSize: 3
        );
        _talk_pool = new ObjectPool<GameObject>(
            createFunc: () => Pool_InstantiateWidget(_talk_prefab),
            actionOnGet: Pool_OnGet,
            actionOnRelease: Pool_OnRelease,
            actionOnDestroy: Pool_OnDestroy,
            collectionCheck: false,
            defaultCapacity: 2,
            maxSize: 3
        );

        Log("OverheadWidgetManager initialized successfully", LogLevel.Info);
    }

    public override void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }

        base.OnDestroy();
    }
    #endregion

    #region Public API
    /// <summary>
    /// Asynchronously retrieves the singleton instance of OverheadWidgetManager.
    /// Waits until the instance is initialized before returning.
    /// </summary>
    /// <returns>The singleton instance</returns>
    /// <exception cref="TaskCanceledException">Thrown if the application stops playing before instance is ready</exception>
    public static async Task<OverheadWidgetManager> GetInstanceAsync()
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        return Instance;
    }

    /// <summary>
    /// Retrieves an overhead widget from the appropriate pool based on type.
    /// The widget will be automatically activated when retrieved.
    /// </summary>
    /// <param name="type">The type of widget to retrieve (Read or Talk)</param>
    /// <returns>The retrieved widget component, or null if the type is invalid</returns>
    public static async Task<WC_OverheadWidget> GetWidget(OverheadWidgetType type)
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        switch (type)
        {
            case OverheadWidgetType.Read:
                return Instance._read_pool.Get().GetComponent<WC_OverheadWidget>();
            case OverheadWidgetType.Talk:
                return Instance._talk_pool.Get().GetComponent<WC_OverheadWidget>();
            default:
                Instance.Log($"Unknown widget type requested: {type}", LogLevel.Error);
                return null;
        }
    }

    /// <summary>
    /// Returns an overhead widget to its pool for reuse.
    /// The widget will be automatically deactivated when returned.
    /// </summary>
    /// <param name="widget">The widget to return to the pool</param>
    public static async void ReturnWidget(WC_OverheadWidget widget)
    {
        while (Instance == null)
        {
            await Task.Yield();

            if (!Application.isPlaying)
                throw new TaskCanceledException();
        }

        if (widget == null)
        {
            Instance.Log("Attempted to return null widget to pool", LogLevel.Warning);
            return;
        }

        switch (widget.WidgetType)
        {
            case OverheadWidgetType.Read:
                Instance._read_pool.Release(widget.gameObject);
                break;
            case OverheadWidgetType.Talk:
                Instance._talk_pool.Release(widget.gameObject);
                break;
            default:
                Instance.Log($"Unknown widget type returned: {widget.WidgetType}", LogLevel.Error);
                break;
        }
    }
    #endregion

    #region IObjectPool API
    /// <summary>
    /// Creates a new widget instance from the specified prefab and parents it to the widget container.
    /// Called by the ObjectPool when a new widget needs to be created.
    /// </summary>
    /// <param name="prefab">The widget prefab to instantiate</param>
    /// <returns>The instantiated widget GameObject</returns>
    private GameObject Pool_InstantiateWidget(WC_OverheadWidget prefab)
    {
        var widget = Instantiate(prefab.gameObject, _widgetCanvas.transform);
        Log($"Created new widget: {prefab.WidgetType}", LogLevel.Verbose);
        return widget;
    }

    /// <summary>
    /// Activates a widget when retrieved from the pool.
    /// Called by the ObjectPool when Get() is invoked.
    /// </summary>
    /// <param name="widget">The widget GameObject to activate</param>
    private void Pool_OnGet(GameObject widget)
    {
        widget.SetActive(true);
    }

    /// <summary>
    /// Deactivates a widget when returned to the pool.
    /// Called by the ObjectPool when Release() is invoked.
    /// </summary>
    /// <param name="widget">The widget GameObject to deactivate</param>
    private void Pool_OnRelease(GameObject widget)
    {
        widget.SetActive(false);
    }

    /// <summary>
    /// Destroys a widget when the pool is full and an item needs to be removed.
    /// Called by the ObjectPool when it exceeds max size.
    /// </summary>
    /// <param name="widget">The widget GameObject to destroy</param>
    private void Pool_OnDestroy(GameObject widget)
    {
        Log($"Destroying excess pooled widget", LogLevel.Verbose);
        Destroy(widget);
    }
    #endregion
}

/// <summary>
/// Defines the types of overhead widgets available in the game.
/// </summary>
public enum OverheadWidgetType
{
    /// <summary>
    /// Widget displayed when an object can be read (e.g., signs, notes)
    /// </summary>
    Read,

    /// <summary>
    /// Widget displayed when an NPC can be talked to
    /// </summary>
    Talk
}