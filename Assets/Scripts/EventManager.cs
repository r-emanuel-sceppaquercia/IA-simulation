using System.Collections.Generic;

public enum EventType
{
    PLAYER_DETECTED,
}

public static class EventManager
{
    public delegate void Methods(params object[] parameters);

    private static Dictionary<EventType, Methods> eventDictionary;

    public static void SubscribeToEvent(EventType eventToSubscribe, Methods methodToSubscribe)
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<EventType, Methods>();
        }

        eventDictionary.TryAdd(eventToSubscribe, null);

        eventDictionary[eventToSubscribe] += methodToSubscribe;
    }

    public static void UnsubscribeToEvent(EventType eventToUnsubscribe, Methods methodToUnsubscribe)
    {
        if (eventDictionary == null) return;
        if (!eventDictionary.ContainsKey(eventToUnsubscribe)) return;

        eventDictionary[eventToUnsubscribe] -= methodToUnsubscribe;
    }

    public static void TriggerEvent(EventType eventToTrigger, params object[] parameters)
    {
        if (eventDictionary == null) return;
        if (!eventDictionary.ContainsKey(eventToTrigger)) return;
        if (eventDictionary[eventToTrigger] == null) return;

        eventDictionary[eventToTrigger].Invoke(parameters);
    }
}
