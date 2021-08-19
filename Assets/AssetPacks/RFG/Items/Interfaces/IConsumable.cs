namespace RFG
{
  public interface IConsumable
  {
    void Consume(Inventory inventory, bool showEffects = true);
  }
}