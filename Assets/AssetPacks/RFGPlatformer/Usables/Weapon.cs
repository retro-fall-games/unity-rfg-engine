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
    public float fireRate = 1f;

    [HideInInspector]
    public StateMachine<WeaponState> weaponState;
    private float _fireRateElapsed = 0f;

    private void Awake()
    {
      weaponState = new StateMachine<WeaponState>(gameObject, true);
      weaponState.OnStateChange += OnStateChange;
      Unequip();
    }

    private void LateUpdate()
    {
      _fireRateElapsed += Time.deltaTime;
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
      if (_fireRateElapsed >= fireRate)
      {
        _fireRateElapsed = 0;
        switch (weaponType)
        {
          case WeaponType.Chargable:
            weaponState.ChangeState(WeaponState.Charging);
            break;
          case WeaponType.InstaFire:
          default:
            weaponState.ChangeState(WeaponState.Firing);
            break;
        }
      }
    }

    public void Charging()
    {
      Debug.Log("Start Charging");
      weaponState.ChangeState(WeaponState.Charged);
    }

    public void Firing()
    {
      Debug.Log("Start Firing");
      Fire();
    }

    public void Charged()
    {
      Debug.Log("Fully Charged");
    }

    public void Fire()
    {
      if (projectile != null)
      {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
      }
      weaponState.ChangeState(WeaponState.Fired);
    }

    public void Fired()
    {
      Debug.Log("Fired");
      Stop();
    }

    public void Stop()
    {
      weaponState.ChangeState(WeaponState.Off);
    }

    private void OnStateChange(WeaponState state)
    {
      // Debug.Log("Weapon State: " + state);
    }

  }

}