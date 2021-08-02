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
    public List<WeaponItem> Inventory => _weapons;
    public Action OnPrimaryEquip;
    public Action OnSecondaryEquip;
    public Action OnPrimaryUnequip;
    public Action OnSecondaryUnequip;
    public Action OnPrimaryFired;
    public Action OnSecondaryFired;

    [HideInInspector]
    private int _equippedPrimaryWeaponIndex;
    private int _equippedSecondaryWeaponIndex;
    private Button _primaryFireButton;
    private Button _secondaryFireButton;
    private float _primaryFireRateElapsed = 0f;
    private float _secondaryFireRateElapsed = 0f;
    private bool _primaryCanFire = false;
    private bool _secondaryCanFire = false;
    private List<WeaponItem> _weapons;
    private List<int> _weaponAmmoCount;

    public override void InitBehaviour()
    {
      _weapons = new List<WeaponItem>();
      _weaponAmmoCount = new List<int>();
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
            if (GetPrimaryAmmoCount() > 0)
            {
              _primaryCanFire = false;
              PrimaryWeapon.Use();
            }
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
            if (GetSecondaryAmmoCount() > 0)
            {
              _secondaryCanFire = false;
              SecondaryWeapon.Use();
            }
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
          AddAmmoPrimary(-1);
          OnPrimaryFired?.Invoke();
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
          AddAmmoSecondary(-1);
          OnSecondaryFired?.Invoke();
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
      if (index < 0 || index >= _weapons.Count)
      {
        LogExt.Warn<WeaponBehaviour>("Cannot equip primary weapon at index: " + index);
        return;
      }

      if (PrimaryWeapon != null)
      {
        PrimaryWeapon.Unequip();
        OnPrimaryUnequip?.Invoke();
      }
      _equippedPrimaryWeaponIndex = index;
      PrimaryWeapon = _weapons[_equippedPrimaryWeaponIndex];
      if (PrimaryWeapon != null)
      {
        PrimaryWeapon.Equip();
        OnPrimaryEquip?.Invoke();
      }
    }

    public void EquipPrimary(WeaponItem weapon)
    {
      int index = _weapons.IndexOf(weapon);
      EquipPrimary(index);
    }

    public void EquipSecondary(int index)
    {
      if (index < 0 || index >= _weapons.Count)
      {
        LogExt.Log<WeaponBehaviour>("Cannot equip secondary weapon at index: " + index);
        return;
      }
      if (SecondaryWeapon != null)
      {
        SecondaryWeapon.Unequip();
        OnSecondaryUnequip?.Invoke();
      }
      _equippedSecondaryWeaponIndex = index;
      SecondaryWeapon = _weapons[_equippedSecondaryWeaponIndex];
      if (SecondaryWeapon != null)
      {
        SecondaryWeapon.Equip();
        OnSecondaryEquip?.Invoke();
      }
    }

    public void EquipSecondary(WeaponItem weapon)
    {
      int index = _weapons.IndexOf(weapon);
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

    public void UpdateAmmoCounts()
    {
      OnPrimaryFired?.Invoke();
      OnSecondaryFired?.Invoke();
    }

    public void AddWeapon(WeaponItem weapon)
    {
      _weapons.Add(weapon);
      _weaponAmmoCount.Add(weapon.startingAmmo);
      EventManager.TriggerEvent<WeaponPickupEvent>(new WeaponPickupEvent(_weapons));
    }

    private void AddAmmoPrimary(int amount)
    {
      int count = _weaponAmmoCount[_equippedPrimaryWeaponIndex];
      int maxAmmo = _weapons[_equippedPrimaryWeaponIndex].maxAmmo;
      count += amount;
      if (count <= 0)
      {
        count = 0;
      }
      else if (count >= maxAmmo)
      {
        count = maxAmmo;
      }
      _weaponAmmoCount[_equippedPrimaryWeaponIndex] = count;
    }

    private void AddAmmoSecondary(int amount)
    {
      int count = _weaponAmmoCount[_equippedSecondaryWeaponIndex];
      int maxAmmo = _weapons[_equippedSecondaryWeaponIndex].maxAmmo;
      count += amount;
      if (count <= 0)
      {
        count = 0;
      }
      else if (count >= maxAmmo)
      {
        count = maxAmmo;
      }
      _weaponAmmoCount[_equippedSecondaryWeaponIndex] = count;
    }

    public void RefillWeapon(WeaponItem weapon)
    {
      int index = _weapons.IndexOf(weapon);
      WeaponItem item = _weapons[index];
      _weaponAmmoCount[index] += item.refillAmmo;
      if (_weaponAmmoCount[index] >= item.maxAmmo)
      {
        _weaponAmmoCount[index] = item.maxAmmo;
      }
    }

    public int GetPrimaryAmmoCount()
    {
      if (_weaponAmmoCount.Count > 0 && _equippedPrimaryWeaponIndex < _weaponAmmoCount.Count)
      {
        return _weaponAmmoCount[_equippedPrimaryWeaponIndex];
      }
      return 0;
    }

    public int GetSecondaryAmmoCount()
    {
      if (_weaponAmmoCount.Count > 0 && _equippedSecondaryWeaponIndex < _weaponAmmoCount.Count)
      {
        return _weaponAmmoCount[_equippedSecondaryWeaponIndex];
      }
      return 0;
    }
  }
}