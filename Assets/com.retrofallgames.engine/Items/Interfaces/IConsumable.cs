namespace RFG.Items
{
  public interface IConsumable
  {
    void Consume(Inventory inventory, bool showEffects = true);
  }
}