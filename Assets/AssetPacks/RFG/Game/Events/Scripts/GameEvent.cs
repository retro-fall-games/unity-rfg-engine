namespace RFG
{
  public struct GameEvent
  {
    public enum GameEventType { Pause, Paused, UnPaused, Quit }
    public GameEventType eventType;
    public GameEvent(GameEventType eventType)
    {
      this.eventType = eventType;
    }
  }
}