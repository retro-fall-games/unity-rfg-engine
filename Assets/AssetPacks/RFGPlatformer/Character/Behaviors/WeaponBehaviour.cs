using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class WeaponBehaviour : CharacterBehaviour
  {
    [Header("Weapons")]
    public List<Weapon> weapons;
    public Weapon PrimaryWeapon { get; private set; }
    public Weapon SecondaryWeapon { get; private set; }

    [HideInInspector]
    private int _equippedPrimaryWeaponIndex;
    private int _equippedSecondaryWeaponIndex;
    private Button _primaryFireButton;
    private Button _secondaryFireButton;

    public override void InitBehaviour()
    {
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput != null);
      yield return new WaitUntil(() => _character.CharacterInput.PrimaryFireButton != null);
      yield return new WaitUntil(() => _character.CharacterInput.SecondaryFireButton != null);

      // Setup buttons
      _primaryFireButton = _character.CharacterInput.PrimaryFireButton;
      _primaryFireButton.State.OnStateChange += PrimaryFireButtonOnStateChanged;

      _secondaryFireButton = _character.CharacterInput.SecondaryFireButton;
      _secondaryFireButton.State.OnStateChange += SecondaryFireButtonOnStateChanged;

      // Setup weapons
      if (weapons.Count == 0)
      {
        weapons = new List<Weapon>();
      }
      else
      {
        int equipOnStart = weapons.FindIndex(0, weapons.Count, w => w.equipOnStart == true);
        if (equipOnStart != -1)
        {
          EquipPrimary(equipOnStart);
        }
      }

    }

    private void PrimaryFireButtonOnStateChanged(ButtonStates state)
    {
      // Weapon equippedWeapon = PrimaryWeapon;
      // if (equippedWeapon == null)
      // {
      //   return;
      // }
      // switch (state)
      // {
      //   case ButtonStates.Down:
      //     if (equippedWeapon.weaponState.CurrentState == Weapon.WeaponState.Off)
      //     {
      //       equippedWeapon.Use();
      //     }
      //     break;
      //   case ButtonStates.Up:
      //     equippedWeapon.Stop();
      //     break;
      // }
    }

    private void SecondaryFireButtonOnStateChanged(ButtonStates state)
    {
      // Weapon equippedWeapon = SecondaryWeapon;
      // if (equippedWeapon == null)
      // {
      //   return;
      // }
      // switch (state)
      // {
      //   case ButtonStates.Down:
      //     if (equippedWeapon.weaponState.CurrentState == Weapon.WeaponState.Off)
      //     {
      //       equippedWeapon.Use();
      //     }
      //     break;
      //   case ButtonStates.Up:
      //     equippedWeapon.Stop();
      //     break;
      // }
    }

    public override void ProcessBehaviour()
    {
      // Weapon primary = PrimaryWeapon;
      // Weapon secondary = SecondaryWeapon;
      // if (primary != null)
      // {
      //   if (primary.weaponState.CurrentState == Weapon.WeaponState.Charging)
      //   {
      //     primary.Charging();
      //   }
      //   else if (primary.weaponState.CurrentState == Weapon.WeaponState.Firing)
      //   {
      //     primary.Firing();
      //   }
      // }
      // if (secondary != null)
      // {
      //   if (secondary.weaponState.CurrentState == Weapon.WeaponState.Charging)
      //   {
      //     secondary.Charging();
      //   }
      //   else if (secondary.weaponState.CurrentState == Weapon.WeaponState.Firing)
      //   {
      //     secondary.Firing();
      //   }
      // }
    }

    public void EquipPrimary(int index)
    {
      if (index < 0 || index >= weapons.Count)
      {
        Debug.Log("Cannot equip primary weapon at index: " + index);
        return;
      }

      if (PrimaryWeapon != null)
      {
        PrimaryWeapon.Unequip();
      }
      _equippedPrimaryWeaponIndex = index;
      PrimaryWeapon = weapons[_equippedPrimaryWeaponIndex];
      if (PrimaryWeapon != null)
      {
        PrimaryWeapon.Equip();
      }
    }

    public void EquipSecondary(int index)
    {
      if (index < 0 || index >= weapons.Count)
      {
        Debug.Log("Cannot equip secondary weapon at index: " + index);
        return;
      }
      if (SecondaryWeapon != null)
      {
        SecondaryWeapon.Unequip();
      }
      _equippedSecondaryWeaponIndex = index;
      SecondaryWeapon = weapons[_equippedSecondaryWeaponIndex];
      if (SecondaryWeapon != null)
      {
        SecondaryWeapon.Equip();
      }
    }

    public void UnequipPrimary()
    {
      if (PrimaryWeapon != null)
      {
        PrimaryWeapon.Unequip();
      }
    }

    public void UnequipSecondary()
    {
      if (SecondaryWeapon != null)
      {
        SecondaryWeapon.Unequip();
      }
    }

  }
}