using System;
using UnityEngine;

namespace RFG.Platformer
{
  using StateMachine;

  [CreateAssetMenu(fileName = "New Death State", menuName = "RFG/Platformer/Character/States/Character State/Death")]
  public class DeathState : State
  {
    public override Type Execute(IStateContext context)
    {
      return typeof(DeadState);
    }
  }
}