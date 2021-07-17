using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/AI Weapon Behaviour")]
  public class AIWeaponBehaviour : CharacterBehaviour
  {
    [Header("Weapons")]
    public List<Weapon> weapons;
    public Weapon PrimaryWeapon { get; private set; }
    public Weapon SecondaryWeapon { get; private set; }

    [HideInInspector]
    private int _equippedPrimaryWeaponIndex = 0;
    private int _equippedSecondaryWeaponIndex = 0;
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

    public override void ProcessBehaviour()
    {
      if (_character.AIState.CurrentState == AIStates.Attacking)
      {
        Weapon primary = PrimaryWeapon;
        Weapon secondary = SecondaryWeapon;
        if (primary != null)
        {
          if (primary.weaponState.CurrentState == Weapon.WeaponState.Charging)
          {
            primary.Charging();
          }
          else if (primary.weaponState.CurrentState == Weapon.WeaponState.Firing)
          {
            primary.Firing();
          }
        }
        if (secondary != null)
        {
          if (secondary.weaponState.CurrentState == Weapon.WeaponState.Charging)
          {
            secondary.Charging();
          }
          else if (secondary.weaponState.CurrentState == Weapon.WeaponState.Firing)
          {
            secondary.Firing();
          }
        }
      }
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