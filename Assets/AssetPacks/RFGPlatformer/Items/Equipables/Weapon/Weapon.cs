using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Items/Equipables/Weapon/Weapon")]
    public class Weapon : MonoBehaviour
    {
      [Header("Settings")]
      public NewWeaponItem WeaponItem;
      public Transform FirePoint;
      public EquipmentSet EquipmentSet;
      public int Ammo;
      public bool IsEquipped;
      public bool IsInCooldown;
      public Action<int> OnAmmoChange;

      [Header("States")]
      public WeaponState[] States;
      public WeaponState DefaultState;
      public WeaponState CurrentState;
      public Type PreviousStateType { get; private set; }
      public Type CurrentStateType { get; private set; }

      public bool CanFire { get { return (_canUse || !IsInCooldown) && Ammo > 0; } }
      public bool IsFiring { get; private set; }

      [HideInInspector]
      private bool _canUse;
      private float _fireRateElapsed;
      private float _cooldownElapsed;
      private float _gainAmmoOverTimeElapsed;
      private Dictionary<Type, WeaponState> _states;

      private void Awake()
      {
        Ammo = WeaponItem.StartingAmmo;
        _states = new Dictionary<Type, WeaponState>();
        foreach (WeaponState state in States)
        {
          _states.Add(state.GetType(), state);
        }
      }

      private void Start()
      {
        Reset();
      }

      private void Update()
      {
        if (!IsEquipped)
        {
          return;
        }
        Type newStateType = CurrentState.Execute(this);
        if (newStateType != null)
        {
          ChangeState(newStateType);
        }
      }

      public void ChangeState(Type newStateType)
      {
        if (_states[newStateType].Equals(CurrentState))
        {
          return;
        }
        if (CurrentState != null)
        {
          PreviousStateType = CurrentState.GetType();
          CurrentState.Exit(this);
        }
        CurrentState = _states[newStateType];
        CurrentStateType = newStateType;
        CurrentState.Enter(this);
      }

      public void Reset()
      {
        CurrentState = null;
        if (DefaultState != null)
        {
          ChangeState(DefaultState.GetType());
        }
      }

      public void RestorePreviousState()
      {
        ChangeState(PreviousStateType);
      }

      private void LateUpdate()
      {
        if (!IsEquipped)
        {
          return;
        }
        _fireRateElapsed += Time.deltaTime;
        if (_fireRateElapsed >= WeaponItem.FireRate)
        {
          _fireRateElapsed = 0;
          _canUse = true;
        }

        if (Ammo <= 0)
        {
          _cooldownElapsed += Time.deltaTime;
          if (_cooldownElapsed >= WeaponItem.Cooldown)
          {
            _cooldownElapsed = 0;
            IsInCooldown = false;
          }
        }

        if (CurrentStateType == typeof(WeaponIdleState) && !IsInCooldown)
        {
          _gainAmmoOverTimeElapsed += Time.deltaTime;
          if (_gainAmmoOverTimeElapsed >= WeaponItem.GainAmmoOverTime)
          {
            _gainAmmoOverTimeElapsed = 0;
            AddAmmo(WeaponItem.AmmoGain);
          }
        }
      }

      public void Started()
      {
        if (!IsEquipped || !CanFire)
        {
          return;
        }
        if (WeaponItem.weaponType == NewWeaponItem.WeaponType.Chargable)
        {
          ChangeState(typeof(WeaponChargingState));
          IsFiring = true;
        }
      }

      public void Cancel()
      {
        if (!IsEquipped || !IsFiring)
        {
          return;
        }
        if (WeaponItem.weaponType == NewWeaponItem.WeaponType.Chargable)
        {
          ChangeState(typeof(WeaponIdleState));
        }
      }

      public void Perform()
      {
        if (!IsEquipped || !CanFire || (WeaponItem.weaponType == NewWeaponItem.WeaponType.Chargable && !IsFiring))
        {
          return;
        }
        ChangeState(typeof(WeaponFiringState));
        AddAmmo(-1);
        _canUse = false;
      }

      private void AddAmmo(int amount)
      {
        Ammo += amount;
        if (Ammo <= 0)
        {
          Ammo = 0;
          IsInCooldown = true;
        }
        else if (Ammo >= WeaponItem.MaxAmmo)
        {
          Ammo = WeaponItem.MaxAmmo;
        }
        OnAmmoChange?.Invoke(Ammo);
      }

      private void RefillAmmo()
      {
        AddAmmo(WeaponItem.RefillAmmo);
      }

      private void OnPickUp(Inventory inventory)
      {
        if (inventory.InInventory(WeaponItem.Id))
        {
          AddAmmo(WeaponItem.RefillAmmo);
        }
      }

      private void OnEquip(Inventory inventory)
      {
        if (EquipmentSet.PrimaryWeapon == null)
        {
          EquipmentSet.EquipPrimaryWeapon(WeaponItem);
          IsEquipped = true;
        }
        else if (!EquipmentSet.PrimaryWeapon.WeaponItem.Equals(WeaponItem) && EquipmentSet.SecondaryWeapon == null)
        {
          EquipmentSet.EquipSecondaryWeapon(WeaponItem);
          IsEquipped = true;
        }
      }

      private void OnUnequip(Inventory inventory)
      {
        IsEquipped = false;
      }

      private void OnEnable()
      {
        WeaponItem.OnPickUp += OnPickUp;
        WeaponItem.OnEquip += OnEquip;
        WeaponItem.OnUnequip += OnUnequip;
      }

      private void OnDisable()
      {
        WeaponItem.OnPickUp -= OnPickUp;
        WeaponItem.OnEquip -= OnEquip;
        WeaponItem.OnUnequip -= OnUnequip;
      }

    }

    public static class WeaponEx
    {
      public static Weapon FindByItem(this List<Weapon> weapons, NewWeaponItem item)
      {
        foreach (Weapon weapon in weapons)
        {
          if (weapon.WeaponItem.Equals(item))
          {
            return weapon;
          }
        }
        return null;
      }
    }
  }
}