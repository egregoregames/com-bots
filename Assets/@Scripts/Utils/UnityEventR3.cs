using System;
using UnityEngine.Events;
using R3;

public class UnityEventR3
{
    private UnityEvent Event = new();
    private Observable<Unit> Observable => Event.AsObservable();
            
    public IDisposable Subscribe(Action x)
    {
        return Observable.Subscribe(y => x());
    }

    public void Invoke()
    {
        Event?.Invoke();
    }
}

public class UnityEventR3<T1>
{
    private UnityEvent<T1> Event = new();
    private Observable<T1> Observable => Event.AsObservable();

    public IDisposable Subscribe(Action x)
    {
        return Observable.Subscribe(y => x());
    }

    public IDisposable Subscribe(Action<T1> x)
    {
        return Observable.Subscribe(x);
    }

    public void Invoke(T1 t1)
    {
        Event?.Invoke(t1);
    }
}