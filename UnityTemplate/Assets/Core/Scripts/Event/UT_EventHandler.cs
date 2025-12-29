using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UT_EventHandler<T> : UT_IEventHandler where T : UT_Event
{
    private readonly Action<T> _Callback;
    private readonly Guid _Guid = Guid.NewGuid();
    public Guid HandlerGuid { get { return _Guid; } }

    public UT_EventHandler(Action<T> InCallback)
    {
        _Callback = InCallback;
    }

    public void Handle(UT_Event Event)
    {
        _Callback.Invoke((T)Event);
    }

    public override bool Equals(object Obj)
    {
        if (Obj is UT_EventHandler<T> Other)
        {
            return _Callback.Equals(Other._Callback);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return _Callback.GetHashCode();
    }
}
