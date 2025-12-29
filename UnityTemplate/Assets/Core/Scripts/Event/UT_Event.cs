using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UT_Event
{
    private static Int64 _EventCount = 0;
    private Int64 _EventId;

    public static Int64 EventCount { get { return _EventCount; } }
    public Int64 EventId { get { return _EventId; } }

    public UT_Event()
    {
        _EventId = ++_EventCount;
    }
}
