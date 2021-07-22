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
    public float fireRate = 1f;

    [Header("Projectile")]
    public Projectile projectile;
    public string objectPoolTag = "";

    [HideInInspector]
    public StateMachine<WeaponState> weaponState;
    private float _fireRateElapsed = 0f;
    private bool _isFacingRight = true;

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

    public void Use(bool isFacingRight)
    {
      _isFacingRight = isFacingRight;
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
      weaponState.ChangeState(WeaponState.Charged);
    }

    public void Firing()
    {
      Fire();
    }

    public void Charged()
    {
    }

    public void Fire()
    {
      if (projectile != null)
      {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
      }
      else if (!objectPoolTag.Equals(""))
      {
        ObjectPool.Instance.SpawnFromPool(objectPoolTag, firePoint.position, firePoint.rotation);
      }
      weaponState.ChangeState(WeaponState.Fired);
    }

    public void Fired()
    {
      Stop();
    }

    public void Stop()
    {
      weaponState.ChangeState(WeaponState.Off);
    }

    private void OnStateChange(WeaponState state)
    {
      // LogExt.Log<Weapon>("Weapon State: " + state);
    }

  }

}