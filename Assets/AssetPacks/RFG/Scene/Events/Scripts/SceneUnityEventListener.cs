using UnityEngine;
using UnityEngine.Events;

namespace RFG
{
  [AddComponentMenu("RFG/Scene/Scene Unity Event Listener")]
  public class SceneUnityEventListener : MonoBehaviour
  {
    public SceneUnityEvent Event;
    public UnityEvent<string> Response;

    private void OnEnable()
    {
      Event.RegisterListener(this);
    }

    private void OnDisable()
    {
      Event.UnregisterListener(this);
    }

    public void OnEventRaised(string name)
    {
      Response.Invoke(name);
    }
  }
}