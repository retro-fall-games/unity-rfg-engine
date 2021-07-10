using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RFG.Events
{
  public static class EventManager
  {
    private static Dictionary<Type, List<EventListenerBase>> _subscribers;
    static EventManager()
    {
      _subscribers = new Dictionary<Type, List<EventListenerBase>>();
    }

    public static void AddListener<Event>(EventListener<Event> listener) where Event : struct
    {
      Type eventType = typeof(Event);
      if (!_subscribers.ContainsKey(eventType))
      {
        _subscribers[eventType] = new List<EventListenerBase>();
      }
      if (!SubscriptionExists(eventType, listener))
      {
        _subscribers[eventType].Add(listener);
      }
    }

    public static void RemoveListener<Event>(EventListener<Event> listener) where Event : struct
    {
      Type eventType = typeof(Event);
      if (!_subscribers.ContainsKey(eventType))
      {
        return;
      }
      List<EventListenerBase> list = _subscribers[eventType];
      list.Remove(listener);
      if (list.Count == 0)
      {
        _subscribers.Remove(eventType);
      }
    }

    public static void TriggerEvent<Event>(Event newEvent) where Event : struct
    {
      List<EventListenerBase> list;
      if (!_subscribers.TryGetValue(typeof(Event), out list))
      {
        return;
      }
      foreach (EventListenerBase item in list)
      {
        (item as EventListener<Event>).OnEvent(newEvent);
      }
    }

    private static bool SubscriptionExists(Type type, EventListenerBase listener)
    {
      List<EventListenerBase> list;
      if (!_subscribers.TryGetValue(type, out list))
      {
        return false;
      }
      foreach (EventListenerBase item in list)
      {
        if (item == listener)
        {
          return true;
        }
      }
      return false;
    }

  }

  public static class EventRegister
  {
    public static void AddListener<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
      EventManager.AddListener<EventType>(caller);
    }
    public static void RemoveListener<EventType>(this EventListener<EventType> caller) where EventType : struct
    {
      EventManager.RemoveListener<EventType>(caller);
    }
  }
}