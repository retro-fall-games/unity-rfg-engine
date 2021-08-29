using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Death State", menuName = "RFG/Platformer/Character/State/Death")]
    public class DeathState : State
    {
      public override Type Execute(Transform transform, Animator animator)
      {
        return typeof(DeadState);
      }
    }
  }
}