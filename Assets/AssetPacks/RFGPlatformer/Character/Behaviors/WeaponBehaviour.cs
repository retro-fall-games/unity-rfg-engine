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
      if (Time.timeScale == 0f)
      {
        return;
      }
      if (PrimaryWeapon == null)
      {
        return;
      }
      switch (state)
      {
        case ButtonStates.Down:
          if (PrimaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Off)
          {
            PrimaryWeapon.Use(_character.Controller.State.IsFacingRight);
          }
          break;
        case ButtonStates.Up:
          PrimaryWeapon.Stop();
          break;
      }
    }

    private void SecondaryFireButtonOnStateChanged(ButtonStates state)
    {
      if (Time.timeScale == 0f)
      {
        return;
      }
      if (SecondaryWeapon == null)
      {
        return;
      }
      switch (state)
      {
        case ButtonStates.Down:
          if (SecondaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Off)
          {
            SecondaryWeapon.Use(_character.Controller.State.IsFacingRight);
          }
          break;
        case ButtonStates.Up:
          SecondaryWeapon.Stop();
          break;
      }
    }

    public override void ProcessBehaviour()
    {
      if (PrimaryWeapon != null)
      {
        if (PrimaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Charging)
        {
          PrimaryWeapon.Charging();
        }
        else if (PrimaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Charged)
        {
          PrimaryWeapon.Charged();
        }
        else if (PrimaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Firing)
        {
          PrimaryWeapon.Firing();
        }
        else if (PrimaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Fired)
        {
          PrimaryWeapon.Fired();
        }
      }
      if (SecondaryWeapon != null)
      {
        if (SecondaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Charging)
        {
          SecondaryWeapon.Charging();
        }
        else if (SecondaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Charged)
        {
          SecondaryWeapon.Charged();
        }
        else if (SecondaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Firing)
        {
          SecondaryWeapon.Firing();
        }
        else if (SecondaryWeapon.weaponState.CurrentState == Weapon.WeaponState.Fired)
        {
          SecondaryWeapon.Fired();
        }
      }
    }

    public void EquipPrimary(int index)
    {
      if (index < 0 || index >= weapons.Count)
      {
        LogExt.Warn<WeaponBehaviour>("Cannot equip primary weapon at index: " + index);
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
        LogExt.Log<WeaponBehaviour>("Cannot equip secondary weapon at index: " + index);
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