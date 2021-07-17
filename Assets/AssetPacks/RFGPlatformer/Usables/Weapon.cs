using UnityEngine;

namespace RFG
{
  public class Weapon : MonoBehaviour, IEquipable, IUsable
  {
    public enum WeaponType { InstaFire, Chargable }
    public enum WeaponState { Off, Charging, Charged, Firing, Fired }

    [Header("Settings")]
    public int damage = 10;
    public bool equipOnStart = false;

    [HideInInspector]
    public StateMachine<WeaponType> weaponType;
    public StateMachine<WeaponState> weaponState;

    private void Awake()
    {
      weaponType = new StateMachine<WeaponType>(gameObject, true);
      Unequip();
    }

    public void Equip()
    {
    }

    public void Unequip()
    {
      weaponState.ChangeState(WeaponState.Off);

    }

    public void Use()
    {
      switch (weaponType.CurrentState)
      {
        case WeaponType.Chargable:
          Charging();
          break;
        case WeaponType.InstaFire:
        default:
          Firing();
          break;
      }
    }

    public virtual void Charging()
    {
      weaponState.ChangeState(WeaponState.Charging);
    }

    public virtual void Firing()
    {
      weaponState.ChangeState(WeaponState.Firing);
    }

    public virtual void Charge()
    {
      weaponState.ChangeState(WeaponState.Charging);
    }

    public virtual void Fire()
    {
      weaponState.ChangeState(WeaponState.Fired);
    }

    public virtual void Stop()
    {
      weaponState.ChangeState(WeaponState.Off);
    }

  }

}