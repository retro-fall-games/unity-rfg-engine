
namespace RFG
{
  public struct PlayerKillEvent
  {
    public BaseCharacter character;
    public PlayerKillEvent(BaseCharacter character)
    {
      this.character = character;
    }
  }
}