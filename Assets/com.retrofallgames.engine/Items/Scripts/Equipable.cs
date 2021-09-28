using System;
using UnityEngine;

namespace RFG.Items
{
  using Core;

  public abstract class Equipable : Consumable, IEquipable, IStorable
  {
    [Header("Equipable Settings")]
    public bool IsEquipped = false;
    public bool EquipOnPickUp = false;
    public Sprite EquipSprite;
    public string EquipText;
    public string UnequipText;
    public string[] EquipEffects;
    public string[] UnequipEffects;

    public Action<Inventory> OnEquip;
    public Action<Inventory> OnUnequip;

    public override bool PickUp(Inventory inventory, bool showEffects = true)
    {
      bool didPickup = base.PickUp(inventory, showEffects);
      if (EquipOnPickUp)
      {
        Equip(inventory);
      }
      return didPickup;
    }

    public virtual void Equip(Inventory inventory)
    {
      IsEquipped = true;
      inventory.transform.SpawnFromPool("Effects", EquipEffects, Quaternion.identity, new object[] { EquipText });
      OnEquip?.Invoke(inventory);
    }

    public virtual void Unequip(Inventory inventory)
    {
      IsEquipped = false;
      inventory.transform.SpawnFromPool("Effects", UnequipEffects, Quaternion.identity, new object[] { UnequipText });
      OnUnequip?.Invoke(inventory);
    }

  }
}