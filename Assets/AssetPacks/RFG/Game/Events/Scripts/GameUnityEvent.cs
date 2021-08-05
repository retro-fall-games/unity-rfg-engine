using System.Collections.Generic;
using UnityEngine;
using MyBox;
namespace RFG
{
  [CreateAssetMenu(fileName = "New Game Event", menuName = "RFG/Game/Game Unity Event")]
  public class GameUnityEvent : ScriptableObject
  {
    private List<GameUnityEventListener> listeners = new List<GameUnityEventListener>();

    public void Raise()
    {
      for (int i = listeners.Count - 1; i >= 0; i--)
      {
        listeners[i].OnEventRaised();
      }
    }

    public void RegisterListener(GameUnityEventListener listener)
    {
      listeners.Add(listener);
    }

    public void UnregisterListener(GameUnityEventListener listenter)
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