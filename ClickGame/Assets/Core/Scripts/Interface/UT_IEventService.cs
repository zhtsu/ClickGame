using System;

public interface UT_IEventService
{
    public void Dispatch<T>(T Event = default) where T : UT_Event, new();

    public void Subscribe<T>(Action<T> Handler) where T : UT_Event;

    public void Unsubscribe<T>(Action<T> Handler) where T : UT_Event;
}
