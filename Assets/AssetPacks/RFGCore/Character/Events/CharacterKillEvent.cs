
namespace RFG
{
  public struct CharacterKillEvent
  {
    public BaseCharacter character;
    public CharacterKillEvent(BaseCharacter character)
    {
      this.character = character;
    }
  }
}