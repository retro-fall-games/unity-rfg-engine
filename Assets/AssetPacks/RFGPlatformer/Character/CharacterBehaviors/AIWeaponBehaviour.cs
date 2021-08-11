using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Platformer/Character/Behaviour/AI Weapon Behaviour")]
    public class AIWeaponBehaviour : CharacterBehaviour
    {
      [Header("Weapons")]
      public List<WeaponItem> weapons;
      public WeaponItem PrimaryWeapon { get; private set; }
      public WeaponItem SecondaryWeapon { get; private set; }

      [HideInInspector]
      private int _equippedPrimaryWeaponIndex = 0;
      private int _equippedSecondaryWeaponIndex = 0;
      // private Button _primaryFireButton;
      // private Button _secondaryFireButton;

      public override void InitBehaviour()
      {
        if (weapons.Count == 0)
        {
          weapons = new List<WeaponItem>();
        }
        else
        {
          // int equipOnStart = weapons.FindIndex(0, weapons.Count, w => w.equipOnStart == true);
          // if (equipOnStart != -1)
          // {
          //   EquipPrimary(equipOnStart);
          // }
        }
      }

      public override void ProcessBehaviour()
      {
        // if (_character.AIState.CurrentState == AIStates.Attacking)
        // {
        //   WeaponItem primary = PrimaryWeapon;
        //   WeaponItem secondary = SecondaryWeapon;
        //   if (primary != null)
        //   {
        //     if (primary.weaponFiringState == WeaponItem.WeaponFiringState.Charging)
        //     {
        //       primary.Charging();
        //     }
        //     else if (primary.weaponFiringState == WeaponItem.WeaponFiringState.Firing)
        //     {
        //       primary.Firing();
        //     }
        //   }
        //   if (secondary != null)
        //   {
        //     if (secondary.weaponFiringState == WeaponItem.WeaponFiringState.Charging)
        //     {
        //       secondary.Charging();
        //     }
        //     else if (secondary.weaponFiringState == WeaponItem.WeaponFiringState.Firing)
        //     {
        //       secondary.Firing();
        //     }
        //   }
        // }
      }

      public void EquipPrimary(int index)
      {
        if (index < 0 || index >= weapons.Count)
        {
          LogExt.Warn<AIWeaponBehaviour>("Cannot equip primary weapon at index: " + index);
          return;
        }

        if (PrimaryWeapon != null)
        {
          PrimaryWeapon.Unequip(null);
        }
        _equippedPrimaryWeaponIndex = index;
        PrimaryWeapon = weapons[_equippedPrimaryWeaponIndex];
        if (PrimaryWeapon != null)
        {
          PrimaryWeapon.Equip(null);
        }
      }

      public void EquipSecondary(int index)
      {
        if (index < 0 || index >= weapons.Count)
        {
          LogExt.Warn<AIWeaponBehaviour>("Cannot equip secondary weapon at index: " + index);
          return;
        }
        if (SecondaryWeapon != null)
        {
          SecondaryWeapon.Unequip(null);
        }
        _equippedSecondaryWeaponIndex = index;
        SecondaryWeapon = weapons[_equippedSecondaryWeaponIndex];
        if (SecondaryWeapon != null)
        {
          SecondaryWeapon.Equip(null);
        }
      }

      public void UnequipPrimary()
      {
        if (PrimaryWeapon != null)
        {
          PrimaryWeapon.Unequip(null);
        }
      }

      public void UnequipSecondary()
      {
        if (SecondaryWeapon != null)
        {
          SecondaryWeapon.Unequip(null);
        }
      }

    }
  }
}