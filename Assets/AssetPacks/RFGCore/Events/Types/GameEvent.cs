
namespace RFG
{
  public struct GameEvent
  {
    public enum GameEventType { Pause }
    public GameEventType eventType;
    public GameEvent(GameEventType eventType)
    {
      this.eventType = eventType;
    }

    private static GameEvent e;
    public static void Trigger(GameEventType eventType)
    {
      e.eventType = eventType;
      EventManager.TriggerEvent(e);
    }
  }
}