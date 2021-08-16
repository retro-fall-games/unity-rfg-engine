using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class EquipmentSet : MonoBehaviour
    {
      public List<Weapon> Weapons;
      public Weapon PrimaryWeapon { get; private set; }
      public Weapon SecondaryWeapon { get; private set; }

      public Action<Weapon> OnEquipPrimaryWeapon;
      public Action<Weapon> OnEquipSecondaryWeapon;
      public Action<Weapon> OnUnequipPrimaryWeapon;
      public Action<Weapon> OnUnequipSecondaryWeapon;

      public void EquipPrimaryWeapon(NewWeaponItem weapon)
      {
        Weapon weaponToEquip = Weapons.FindByItem(weapon);
        if (weaponToEquip != null)
        {
          if (PrimaryWeapon != null)
          {
            OnUnequipPrimaryWeapon?.Invoke(PrimaryWeapon);
          }
          PrimaryWeapon = weaponToEquip;
          OnEquipPrimaryWeapon?.Invoke(PrimaryWeapon);
        }
      }

      public void EquipSecondaryWeapon(NewWeaponItem weapon)
      {
        Weapon weaponToEquip = Weapons.FindByItem(weapon);
        if (weaponToEquip != null)
        {
          if (SecondaryWeapon != null)
          {
            OnUnequipSecondaryWeapon?.Invoke(SecondaryWeapon);
          }
          SecondaryWeapon = weaponToEquip;
          OnEquipSecondaryWeapon?.Invoke(SecondaryWeapon);
        }
      }
    }
  }
}