using System;
using UnityEngine;

namespace RFG
{
  public abstract class Consumable : Item, IConsumable, IStorable, IStackable
  {
    [Header("Consumable Settings")]
    public bool ConsumeOnPickUp = false;
    public string ConsumeText;
    public string[] ConsumeEffects;
    public Action<Inventory> OnConsume;

    public override bool PickUp(Inventory inventory)
    {
      bool didPickup = base.PickUp(inventory);
      if (ConsumeOnPickUp)
      {
        Consume(inventory);
        return false;
      }
      return didPickup;
    }

    public virtual void Consume(Inventory inventory)
    {
      if (ConsumeEffects.Length > 0)
      {
        foreach (string effect in ConsumeEffects)
        {
          ObjectPoolManager.Instance.SpawnFromPool("Effects", effect, inventory.transform.position, Quaternion.identity, null, false, new object[] { ConsumeText });
        }
      }
      OnConsume?.Invoke(inventory);
    }

  }
}