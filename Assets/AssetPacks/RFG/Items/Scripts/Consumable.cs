using UnityEngine;

namespace RFG
{
  public abstract class Consumable : Item, IConsumable, IStorable, IStackable
  {
    [Header("Consumable Settings")]
    public bool ConsumeOnPickUp = false;

    public override bool OnPickUp(Inventory inventory)
    {
      bool didPickup = base.OnPickUp(inventory);
      if (ConsumeOnPickUp)
      {
        Use(inventory);
        return false;
      }
      return didPickup;
    }

    public virtual void Use(Inventory inventory)
    {
    }

  }
}