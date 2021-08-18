using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {

    [Serializable]
    public class EquipmentSetSave
    {
      public WeaponItemSave PrimaryWeapon;
      public WeaponItemSave SecondaryWeapon;
    }

    public class EquipmentSet : MonoBehaviour
    {
      public WeaponItem PrimaryWeapon { get; private set; }
      public WeaponItem SecondaryWeapon { get; private set; }

      public Action<WeaponItem> OnEquipPrimaryWeapon;
      public Action<WeaponItem> OnEquipSecondaryWeapon;
      public Action<WeaponItem> OnUnequipPrimaryWeapon;
      public Action<WeaponItem> OnUnequipSecondaryWeapon;

      public void EquipPrimaryWeapon(WeaponItem weapon)
      {
        if (weapon != null)
        {
          if (PrimaryWeapon != null)
          {
            OnUnequipPrimaryWeapon?.Invoke(PrimaryWeapon);
          }
          PrimaryWeapon = weapon;
          OnEquipPrimaryWeapon?.Invoke(PrimaryWeapon);
        }
      }

      public void EquipSecondaryWeapon(WeaponItem weapon)
      {
        if (weapon != null)
        {
          if (SecondaryWeapon != null)
          {
            OnUnequipSecondaryWeapon?.Invoke(SecondaryWeapon);
          }
          SecondaryWeapon = weapon;
          OnEquipSecondaryWeapon?.Invoke(SecondaryWeapon);
        }
      }

      public EquipmentSetSave GetSave()
      {
        EquipmentSetSave save = new EquipmentSetSave();
        if (PrimaryWeapon != null)
        {
          save.PrimaryWeapon = PrimaryWeapon.GetWeaponSave();
        }
        if (SecondaryWeapon != null)
        {
          save.SecondaryWeapon = SecondaryWeapon.GetWeaponSave();
        }
        return save;
      }

      public void RestoreSave(EquipmentSetSave save)
      {
        WeaponItem primary = (WeaponItem)save.PrimaryWeapon.Guid.FindObject();
        WeaponItem secondary = (WeaponItem)save.SecondaryWeapon.Guid.FindObject();
        if (primary != null)
        {
          primary.Ammo = save.PrimaryWeapon.Ammo;
          if (primary.IsEquipped)
          {
            EquipPrimaryWeapon(primary);
          }
        }
        if (secondary != null)
        {
          secondary.Ammo = save.SecondaryWeapon.Ammo;
          if (secondary.IsEquipped)
          {
            EquipSecondaryWeapon(secondary);
          }
        }
      }
    }
  }
}