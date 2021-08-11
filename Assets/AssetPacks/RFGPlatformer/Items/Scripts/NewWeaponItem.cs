using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Weapon Item", menuName = "RFG/Platformer/Items/Consumable/Weapon")]
    public class NewWeaponItem : Consumable
    {
      public enum WeaponType { InstaFire, Chargable }

      [Header("Settings")]
      public WeaponType weaponType = WeaponType.InstaFire;
      public float FireRate = 1f;
      public string[] ProjectileFX;

      [Header("Ammo")]
      public int MaxAmmo = 100;
      public int StartingAmmo = 10;
      public int RefillAmmo = 10;

      public override void Use(Inventory inventory)
      {

      }

    }
  }
}