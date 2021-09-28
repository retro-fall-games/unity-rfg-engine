using System;
using UnityEngine;

namespace RFG.Platformer
{
  using Core;

  [CreateAssetMenu(fileName = "New Weapon Firing State", menuName = "RFG/Platformer/Items/Equipable/Weapon/States/Firing")]
  public class WeaponFiringState : WeaponState
  {
    public string[] Projectiles;

    public override Type Execute(Weapon weapon)
    {
      weapon.FirePoint.SpawnFromPool("Projectiles", Projectiles);
      return typeof(WeaponFiredState);
    }
  }
}