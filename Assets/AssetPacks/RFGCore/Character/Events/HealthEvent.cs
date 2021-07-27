namespace RFG
{
  public struct HealthEvent
  {
    public enum HealthEventType { Heal, Damage, Death, Reset }
    public BaseCharacter character;
    public HealthBehaviour health;
    public HealthEventType type;
    public HealthEvent(BaseCharacter character, HealthBehaviour health, HealthEventType type)
    {
      this.character = character;
      this.health = health;
      this.type = type;
    }
  }
}