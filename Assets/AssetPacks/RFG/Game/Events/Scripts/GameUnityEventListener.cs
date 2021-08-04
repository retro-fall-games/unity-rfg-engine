using UnityEngine;
using UnityEngine.Events;

namespace RFG
{
  [AddComponentMenu("RFG/Game/Game Unity Event Listener")]
  public class GameUnityEventListener : MonoBehaviour
  {
    public GameUnityEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
      Event.RegisterListener(this);
    }

    private void OnDisable()
    {
      Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
      Response.Invoke();
    }
  }
}