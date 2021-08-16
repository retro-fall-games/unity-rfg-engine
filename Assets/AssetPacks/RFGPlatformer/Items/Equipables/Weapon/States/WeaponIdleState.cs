using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Weapon Idle State", menuName = "RFG/Platformer/Items/Equipable/Weapon/States/Idle")]
    public class WeaponIdleState : WeaponState
    {
      public override Type Execute(Weapon weapon)
      {
        return null;
      }
    }
  }
}