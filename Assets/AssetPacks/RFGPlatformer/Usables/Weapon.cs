using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Usables/Weapon")]
  public class Weapon : MonoBehaviour, IEquipable, IUsable
  {
    public enum WeaponType { InstaFire, Chargable }
    public enum WeaponState { Off, Charging, Charged, Firing, Fired }

    [Header("Settings")]
    public WeaponType weaponType = WeaponType.InstaFire;
    public int damage = 10;
    public bool equipOnStart = false;
    public Transform firePoint;
    public Projectile projectile;

    [HideInInspector]
    public StateMachine<WeaponState> weaponState;

    private void Awake()
    {
      weaponState = new StateMachine<WeaponState>(gameObject, true);
      weaponState.OnStateChange += OnStateChange;
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
      switch (weaponType)
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

    public void Charging()
    {
      weaponState.ChangeState(WeaponState.Charging);
    }

    public void Firing()
    {
      weaponState.ChangeState(WeaponState.Firing);
      StartCoroutine(FiringCo());
    }

    private IEnumerator FiringCo()
    {
      yield return new WaitForEndOfFrame();
      Fire();
    }

    public void Charge()
    {
      weaponState.ChangeState(WeaponState.Charging);
    }

    public void Fire()
    {
      if (projectile != null)
      {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
      }
      StartCoroutine(FireCo());
    }

    private IEnumerator FireCo()
    {
      yield return new WaitForEndOfFrame();
      weaponState.ChangeState(WeaponState.Fired);
    }

    public void Stop()
    {
      weaponState.ChangeState(WeaponState.Off);
    }

    private void OnStateChange(WeaponState state)
    {
      Debug.Log("Weapon State: " + state);
    }

  }

}