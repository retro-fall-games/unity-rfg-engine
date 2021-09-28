using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Game Event", menuName = "RFG/Core/Events/Game Event")]
  public class GameEvent : ScriptableObject
  {
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
      for (int i = listeners.Count - 1; i >= 0; i--)
      {
        listeners[i].OnEventRaised();
      }
    }

    public void RegisterListener(GameEventListener listener)
    {
      listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listenter)
    {
      listeners.Remove(listenter);
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void RaiseEvent()
    {
      Raise();
    }
#endif
  }
}