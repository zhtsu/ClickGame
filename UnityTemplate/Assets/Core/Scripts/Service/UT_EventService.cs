using Cysharp.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class UT_EventService : UT_Service, UT_IEventService
{
    public override string ServiceName => "Event Service";

    private readonly ConcurrentQueue<UT_Event> _EventQueue = new();
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, UT_IEventHandler>> _HandlersDict = new();

    public override UniTask Initialize()
    {
        _EventQueue.Clear();
        _HandlersDict.Clear();

        return UniTask.CompletedTask;
    }

    public override void Destroy()
    {
        _EventQueue.Clear();
        _HandlersDict.Clear();
    }

    public void Update()
    {
        if (_EventQueue.Count == 0)
            return;

        if (_EventQueue.TryDequeue(out UT_Event Event))
        {
            Type EventType = Event.GetType();

            if (_HandlersDict.TryGetValue(EventType, out ConcurrentDictionary<Guid, UT_IEventHandler> HandlerDict))
            {
                foreach (UT_IEventHandler Handler in HandlerDict.Values)
                {
                    Handler.Handle(Event);
                }
            }
        }
    }

    public void Dispatch<T>(T Event = default) where T : UT_Event, new()
    {
        _EventQueue.Enqueue(Event);
    }

    public void Subscribe<T>(Action<T> InAction) where T : UT_Event
    {
        Type EventType = typeof(T);

        ConcurrentDictionary<Guid, UT_IEventHandler> HandlerDict = _HandlersDict.GetOrAdd(EventType, (Key) =>
        {
            return new ConcurrentDictionary<Guid, UT_IEventHandler>();
        });

        UT_EventHandler<T> NewEventHandler = new UT_EventHandler<T>(InAction);
        if (FoundHandler(HandlerDict.Values, NewEventHandler, out Guid Temp))
            return;

        HandlerDict.TryAdd(NewEventHandler.HandlerGuid, NewEventHandler);
    }

    public void Unsubscribe<T>(Action<T> InAction) where T : UT_Event
    {
        Type EventType = typeof(T);

        if (_HandlersDict.TryGetValue(EventType, out ConcurrentDictionary<Guid, UT_IEventHandler> HandlerDict))
        {
            UT_EventHandler<T> NewEventHandler = new UT_EventHandler<T>(InAction);
            if (FoundHandler(HandlerDict.Values, NewEventHandler, out Guid FoundGuid))
            {
                HandlerDict.TryRemove(FoundGuid, out UT_IEventHandler Temp);
            }
        }
    }

    private bool FoundHandler(ICollection<UT_IEventHandler> HandlerCollection, UT_IEventHandler SearchedHandler, out Guid FoundGuid)
    {
        foreach (UT_IEventHandler Handler in HandlerCollection)
        {
            if (Handler.Equals(SearchedHandler))
            {
                FoundGuid = Handler.HandlerGuid;
                return true;
            }
        }

        FoundGuid = Guid.Empty;
        return false;
    }
}
