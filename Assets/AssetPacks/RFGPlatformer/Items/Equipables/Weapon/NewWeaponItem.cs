using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "RFG/Platformer/Items/Equipable/Weapon")]
    public class NewWeaponItem : Equipable
    {
      public enum WeaponType { InstaFire, Chargable }

      [Header("Settings")]
      public WeaponType weaponType = WeaponType.InstaFire;
      public float FireRate = 1f;
      public float Cooldown = 0f;

      [Header("Ammo")]
      public int MaxAmmo = 100;
      public int StartingAmmo = 10;
      public int RefillAmmo = 10;
      public float GainAmmoOverTime = 0;
      public int AmmoGain = 0;

    }
  }
}