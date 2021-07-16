
namespace RFG
{
  public struct InputEvent
  {
    public EventType eventType;
    public InputEvent(EventType eventType)
    {
      this.eventType = eventType;
    }

    private static InputEvent e;
    public static void Trigger(EventType eventType)
    {
      e.eventType = eventType;
      EventManager.TriggerEvent(e);
    }
  }
}