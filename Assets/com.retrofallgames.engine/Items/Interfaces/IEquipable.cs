namespace RFG.Items
{
  public interface IEquipable
  {
    void Equip(Inventory inventory);
    void Unequip(Inventory inventory);
  }
}