using System.Collections.Generic;
using UnityEngine;
using R3;
using System;

public class MonoBehaviourR3 : MonoBehaviour
{
    [NonSerialized]
    protected bool _initialized;

    [field: SerializeField]
    protected LogLevel Logging { get; set; } = LogLevel.Info;

    protected List<IDisposable> Events { get; } = new();

    public bool IsDestroyed { get; private set; }

    protected void Awake()
    {
        TryInitialize();
    }

    protected void OnEnable()
    {
        TryInitialize();
    }

    public virtual void OnDestroy()
    {
        foreach (var item in Events)
        {
            item.Dispose();
        }

        IsDestroyed = true;
    }

    protected void AddEvents(params IDisposable[] events)
    {
        Events.AddRange(events);
    }

    private void TryInitialize()
    {
        if (_initialized) return;
        _initialized = true;
        Initialize();
    }

    /// <summary>
    /// Hot-reload compatible initialization code. Can be used in place of Awake
    /// in many cases, and OnEnable in some cases
    /// </summary>
    protected virtual void Initialize()
    {
        
    }

    protected enum LogLevel
    {
        None,
        Exception,
        Error,
        Warning,
        Info,
        Verbose
    }

    protected void Log(string message, LogLevel logLevel = LogLevel.Info)
    {
        if (logLevel == 0)
        {
            Debug.LogError("Do not use LogLevel.None");
            return;
        }

        if (Logging < logLevel)
            return;

        if (logLevel == LogLevel.Exception)
        {
            Debug.LogException(new Exception(message));
        }
        else if (logLevel == LogLevel.Error)
        {
            Debug.LogError(message);
        }
        else if (logLevel == LogLevel.Warning)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    protected void Log(Exception exception, LogLevel logLevel = LogLevel.Exception)
    {
        if (logLevel == 0)
        {
            Debug.LogError("Do not use LogLevel.None");
            return;
        }

        if (Logging < logLevel)
            return;

        if (logLevel == LogLevel.Exception)
        {
            Debug.LogException(exception);
        }
        else if (logLevel == LogLevel.Error)
        {
            Debug.LogError(exception.Message);
        }
        else if (logLevel == LogLevel.Warning)
        {
            Debug.LogWarning(exception.Message);
        }
        else
        {
            Debug.Log(exception.Message);
        }
    }
}
