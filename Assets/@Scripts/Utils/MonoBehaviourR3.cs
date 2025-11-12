using System.Collections.Generic;
using UnityEngine;
using R3;
using System;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class MonoBehaviourR3 : MonoBehaviour
{
    public class InputsR3 : IDisposable
    {
        private InputSystem_Actions Inputs { get; set; }
        private Observable<InputAction.CallbackContext> _uiRight;
        private Observable<InputAction.CallbackContext> _uiDown;
        private Observable<InputAction.CallbackContext> _uiLeft;
        private Observable<InputAction.CallbackContext> _uiUp;
        private Observable<InputAction.CallbackContext> _uiSubmit;
        private Observable<InputAction.CallbackContext> _uiCancel;

        public void TryEnable()
        {
            Inputs?.Enable();
        }

        public void TryDisable()
        {
            Inputs?.Disable();
        }

        private void TryInitialize()
        {
            if (Inputs == null)
            {
                Inputs = new InputSystem_Actions();
                Inputs.Enable();
            }
        }

        private IDisposable Subscribe(Func<InputAction> getInputAction, 
            Observable<InputAction.CallbackContext> observable, 
            Action<InputAction.CallbackContext> x)
        {
            TryInitialize();

            var inputAction = getInputAction();

            observable ??= Observable.FromEvent<InputAction.CallbackContext>(
                h => inputAction.performed += h,
                h => inputAction.performed -= h);

            return observable.Subscribe(x);
        }

        public IDisposable UI_Right(Action<InputAction.CallbackContext> x) 
            => Subscribe(() => Inputs.UI.Right, _uiRight, x);

        public IDisposable UI_Left(Action<InputAction.CallbackContext> x)
            => Subscribe(() => Inputs.UI.Left, _uiLeft, x);

        public IDisposable UI_Down(Action<InputAction.CallbackContext> x)
            => Subscribe(() => Inputs.UI.Down, _uiDown, x);

        public IDisposable UI_Up(Action<InputAction.CallbackContext> x)
            => Subscribe(() => Inputs.UI.Up, _uiUp, x);

        public IDisposable UI_Submit(Action<InputAction.CallbackContext> x)
            => Subscribe(() => Inputs.UI.Submit, _uiSubmit, x);

        public IDisposable UI_Cancel(Action<InputAction.CallbackContext> x)
            => Subscribe(() => Inputs.UI.Cancel, _uiCancel, x);

        public void Dispose()
        {
            Inputs?.Dispose();
        }
    }

    [NonSerialized]
    protected bool _initialized;

    [field: SerializeField]
    protected LogLevel Logging { get; set; } = LogLevel.Info;

    protected List<IDisposable> Events { get; } = new();

    public bool IsDestroyed { get; private set; }

    public InputsR3 Inputs { get; private set; } = new();

    protected void Awake()
    {
        TryInitialize();
    }

    protected void OnEnable()
    {
        Inputs.TryEnable();
        TryInitialize();
    }

    protected void OnDisable()
    {
        Inputs.TryDisable();
    }

    public virtual void OnDestroy()
    {
        Inputs.Dispose();
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