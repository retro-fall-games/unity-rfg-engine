using System;
using UnityEngine;

namespace RFG.Items
{
  using Core;

  public abstract class Consumable : Item, IConsumable, IStorable, IStackable
  {
    [Header("Consumable Settings")]
    public bool ConsumeOnPickUp = false;
    public bool AddToInventory = false;
    public string ConsumeText;
    public string[] ConsumeEffects;
    public Action<Inventory> OnConsume;

    public override bool PickUp(Inventory inventory, bool showEffects = true)
    {
      bool didPickup = base.PickUp(inventory, showEffects);
      if (ConsumeOnPickUp)
      {
        Consume(inventory, showEffects);
        return AddToInventory;
      }
      return didPickup;
    }

    public virtual void Consume(Inventory inventory, bool showEffects = true)
    {
      if (showEffects)
      {
        inventory.transform.SpawnFromPool("Effects", ConsumeEffects, Quaternion.identity, new object[] { ConsumeText });
      }
      OnConsume?.Invoke(inventory);
    }

  }
}