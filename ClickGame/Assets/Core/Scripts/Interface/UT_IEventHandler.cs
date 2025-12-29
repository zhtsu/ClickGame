using System;
using UnityEngine;

public interface UT_IEventHandler
{
    public void Handle(UT_Event Event);
    public Guid HandlerGuid { get; }
}
