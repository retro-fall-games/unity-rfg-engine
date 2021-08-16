using System;
using UnityEngine;

namespace RFG
{
  public abstract class Equipable : Consumable, IEquipable, IStorable
  {
    [Header("Equipable Settings")]
    public bool EquipOnPickUp = false;
    public Sprite EquipSprite;
    public string EquipText;
    public string UnequipText;
    public string[] EquipEffects;
    public string[] UnequipEffects;

    public Action<Inventory> OnEquip;
    public Action<Inventory> OnUnequip;

    public override bool PickUp(Inventory inventory)
    {
      base.PickUp(inventory);
      if (EquipOnPickUp)
      {
        Equip(inventory);
      }
      return true;
    }

    public virtual void Equip(Inventory inventory)
    {
      if (EquipEffects.Length > 0)
      {
        foreach (string effect in EquipEffects)
        {
          ObjectPoolManager.Instance.SpawnFromPool("Effects", effect, inventory.transform.position, Quaternion.identity, null, false, new object[] { EquipText });
        }
      }
      OnEquip?.Invoke(inventory);
    }

    public virtual void Unequip(Inventory inventory)
    {
      if (UnequipEffects.Length > 0)
      {
        foreach (string effect in UnequipEffects)
        {
          ObjectPoolManager.Instance.SpawnFromPool("Effects", effect, inventory.transform.position, Quaternion.identity, null, false, new object[] { UnequipText });
        }
      }
      OnUnequip?.Invoke(inventory);
    }

  }
}