using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class WeaponBehaviour : BaseCharacterBehaviour
  {
    public WeaponItem PrimaryWeapon { get; set; }
    public WeaponItem SecondaryWeapon { get; set; }
    public Action OnPrimaryEquip;
    public Action OnSecondaryEquip;

    [HideInInspector]
    private int _equippedPrimaryWeaponIndex;
    private int _equippedSecondaryWeaponIndex;
    private Button _primaryFireButton;
    private Button _secondaryFireButton;
    private float _primaryFireRateElapsed = 0f;
    private float _secondaryFireRateElapsed = 0f;
    private bool _primaryCanFire = false;
    private bool _secondaryCanFire = false;

    public override void InitBehaviour()
    {
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => InputManager.Instance != null);
      yield return new WaitUntil(() => InputManager.Instance.PrimaryFireButton != null);
      yield return new WaitUntil(() => InputManager.Instance.SecondaryFireButton != null);

      // Setup buttons
      _primaryFireButton = InputManager.Instance.PrimaryFireButton;
      _primaryFireButton.State.OnStateChange += PrimaryFireButtonOnStateChanged;

      _secondaryFireButton = InputManager.Instance.SecondaryFireButton;
      _secondaryFireButton.State.OnStateChange += SecondaryFireButtonOnStateChanged;
    }

    private void PrimaryFireButtonOnStateChanged(ButtonStates state)
    {
      if (Time.timeScale == 0f || PrimaryWeapon == null || !_primaryCanFire)
      {
        return;
      }
      switch (state)
      {
        case ButtonStates.Down:
          if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Off)
          {
            _primaryCanFire = false;
            PrimaryWeapon.Use();
          }
          break;
        case ButtonStates.Up:
          PrimaryWeapon.Stop();
          break;
      }
    }

    private void SecondaryFireButtonOnStateChanged(ButtonStates state)
    {
      if (Time.timeScale == 0f || SecondaryWeapon == null || !_secondaryCanFire)
      {
        return;
      }
      switch (state)
      {
        case ButtonStates.Down:
          if (SecondaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Off)
          {
            _secondaryCanFire = false;
            SecondaryWeapon.Use();
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
        if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charging)
        {
          PrimaryWeapon.Charging();
        }
        else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charged)
        {
          PrimaryWeapon.Charged();
        }
        else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Firing)
        {
          PrimaryWeapon.Firing();
        }
        else if (PrimaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Fired)
        {
          PrimaryWeapon.Fired();
        }
        _primaryFireRateElapsed += Time.deltaTime;
        if (_primaryFireRateElapsed >= PrimaryWeapon.fireRate)
        {
          _primaryFireRateElapsed = 0;
          _primaryCanFire = true;
        }
      }
      if (SecondaryWeapon != null)
      {
        if (SecondaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charging)
        {
          SecondaryWeapon.Charging();
        }
        else if (SecondaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Charged)
        {
          SecondaryWeapon.Charged();
        }
        else if (SecondaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Firing)
        {
          SecondaryWeapon.Firing();
        }
        else if (SecondaryWeapon.weaponFiringState == WeaponItem.WeaponFiringState.Fired)
        {
          SecondaryWeapon.Fired();
        }
        _secondaryFireRateElapsed += Time.deltaTime;
        if (_secondaryFireRateElapsed >= SecondaryWeapon.fireRate)
        {
          _secondaryFireRateElapsed = 0;
          _secondaryCanFire = true;
        }
      }
    }

    public void EquipPrimary(int index)
    {
      List<WeaponItem> weapons = _baseCharacter.WeaponInventory.GetAll();
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
        OnPrimaryEquip?.Invoke();
      }
    }

    public void EquipPrimary(WeaponItem weapon)
    {
      int index = _baseCharacter.WeaponInventory.IndexOf(weapon);
      EquipPrimary(index);
    }

    public void EquipSecondary(int index)
    {
      List<WeaponItem> weapons = _baseCharacter.WeaponInventory.GetAll();
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
        OnSecondaryEquip?.Invoke();
      }
    }

    public void EquipSecondary(WeaponItem weapon)
    {
      int index = _baseCharacter.WeaponInventory.IndexOf(weapon);
      EquipSecondary(index);
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