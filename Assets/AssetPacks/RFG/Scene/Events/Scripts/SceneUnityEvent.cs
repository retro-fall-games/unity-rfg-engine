using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Scene Event", menuName = "RFG/Scene/Scene Unity Event")]
  public class SceneUnityEvent : ScriptableObject
  {
    private List<SceneUnityEventListener> listeners = new List<SceneUnityEventListener>();

    public void Raise(string name)
    {
      for (int i = listeners.Count - 1; i >= 0; i--)
      {
        listeners[i].OnEventRaised(name);
      }
    }

    public void RegisterListener(SceneUnityEventListener listener)
    {
      listeners.Add(listener);
    }

    public void UnregisterListener(SceneUnityEventListener listenter)
    {
      listeners.Remove(listenter);
    }

#if UNITY_EDITOR
    public string RaisEventTest;
    [ButtonMethod]
    private void RaiseEvent()
    {
      Raise(RaisEventTest);
    }
#endif
  }
}