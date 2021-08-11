using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "RFG Engine/Items/Weapon")]
    public class WeaponItem : ScriptableObject, IEquipable, IUsable
    {
      public enum WeaponType { InstaFire, Chargable }
      public enum WeaponState { Equiped, Unequiped }
      public enum WeaponFiringState { Off, Charging, Charged, Firing, Fired }

      [Header("Save Data")]
      public int id;

      [Header("States")]
      public WeaponType weaponType = WeaponType.InstaFire;
      public WeaponState weaponState = WeaponState.Unequiped;
      public WeaponFiringState weaponFiringState = WeaponFiringState.Off;

      [Header("Settings")]
      public float fireRate = 1f;
      public Transform firePoint;
      public Projectile projectile;
      public string objectPoolTag = "";
      public Sprite pickupSprite;
      public string pickupText;

      [Header("Ammo")]
      public int maxAmmo = 100;
      public int startingAmmo = 10;
      public int refillAmmo = 10;
      public string pickupAmmoText;


      public void Equip(Inventory i)
      {
        weaponState = WeaponState.Equiped;
      }

      public void Unequip(Inventory i)
      {
        weaponFiringState = WeaponFiringState.Off;
        weaponState = WeaponState.Unequiped;
      }

      public void Use()
      {
        switch (weaponType)
        {
          case WeaponItem.WeaponType.Chargable:
            weaponFiringState = WeaponFiringState.Charging;
            break;
          case WeaponItem.WeaponType.InstaFire:
          default:
            weaponFiringState = WeaponFiringState.Firing;
            break;
        }
      }

      public void Charging()
      {
        weaponFiringState = WeaponFiringState.Charged;
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
        weaponFiringState = WeaponFiringState.Fired;
      }

      public void Fired()
      {
        Stop();
      }

      public void Stop()
      {
        weaponFiringState = WeaponFiringState.Off;
      }

    }
  }
}