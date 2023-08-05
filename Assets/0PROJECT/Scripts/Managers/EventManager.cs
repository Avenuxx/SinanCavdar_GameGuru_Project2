using System;
using System.Collections.Generic;

public enum GameEvent
{
    OnStart,
    OnLose,
    OnWin,
    OnNextLevel,
    OnPlaceStack,
    OnSpawnStack,
    OnSetFinishLine,
    OnCollect,

    //AUDIO EVENTS
    OnPlaySound,
    OnPlaySoundPitch,
}

public static class EventManager
{
    private static Dictionary<GameEvent, Action> eventTable =
        new Dictionary<GameEvent, Action>();

    public static void AddHandler(GameEvent gameEvent, Action action)
    {
        if (!eventTable.ContainsKey(gameEvent))
            eventTable[gameEvent] = action;
        else eventTable[gameEvent] += action;
    }

    public static void RemoveHandler(GameEvent gameEvent, Action action)
    {
        if (eventTable[gameEvent] != null)
            eventTable[gameEvent] -= action;

        if (eventTable[gameEvent] == null)
            eventTable.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent)
    {
        if (eventTable[gameEvent] != null)
            eventTable[gameEvent]();
    }

    private static Dictionary<GameEvent, Action<object>> eventTableFloat
        = new Dictionary<GameEvent, Action<object>>();

    public static void AddHandler(GameEvent gameEvent, Action<object> action)
    {
        if (!eventTableFloat.ContainsKey(gameEvent)) eventTableFloat[gameEvent]
                 = action;
        else eventTableFloat[gameEvent] += action;
    }

    public static void RemoveHandler(GameEvent gameEvent, Action<object> action)
    {
        if (eventTableFloat[gameEvent] != null)
            eventTableFloat[gameEvent] -= action;

        if (eventTableFloat[gameEvent] == null)
            eventTableFloat.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent, object value)
    {
        if (eventTableFloat[gameEvent] != null)
            eventTableFloat[gameEvent](value);
    }



    private static Dictionary<GameEvent, Action<object, object>> eventTableTriple
        = new Dictionary<GameEvent, Action<object, object>>();

    public static void AddHandler(GameEvent gameEvent, Action<object, object> action)
    {
        if (!eventTableTriple.ContainsKey(gameEvent)) eventTableTriple[gameEvent]
                 = action;
        else eventTableTriple[gameEvent] += action;
    }

    public static void RemoveHandler(GameEvent gameEvent, Action<object, object> action)
    {
        if (eventTableTriple[gameEvent] != null)
            eventTableTriple[gameEvent] -= action;

        if (eventTableTriple[gameEvent] == null)
            eventTableTriple.Remove(gameEvent);
    }

    public static void Broadcast(GameEvent gameEvent, object value1, object value2)
    {
        if (eventTableTriple[gameEvent] != null)
            eventTableTriple[gameEvent](value1, value2);
    }
}
