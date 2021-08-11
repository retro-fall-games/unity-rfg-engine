using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class Weapon : MonoBehaviour
    {
      public NewWeaponItem WeaponItem;

      public enum WeaponFiringState { Off, Charging, Charged, Firing, Fired }
      public WeaponFiringState weaponFiringState = WeaponFiringState.Off;

      public Transform FirePoint;

      public void Use()
      {
        // switch (WeaponItem.weaponType)
        // {
        //   case WeaponItem.WeaponType.Chargable:
        //     weaponFiringState = WeaponFiringState.Charging;
        //     break;
        //   case WeaponItem.WeaponType.InstaFire:
        //   default:
        //     weaponFiringState = WeaponFiringState.Firing;
        //     break;
        // }
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
        if (WeaponItem.ProjectileFX.Length > 0)
        {
          foreach (string fx in WeaponItem.ProjectileFX)
          {
            ObjectPool.Instance.SpawnFromPool(fx, FirePoint.position, Quaternion.identity);
          }
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