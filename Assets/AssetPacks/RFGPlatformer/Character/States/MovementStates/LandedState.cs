using System;
using UnityEngine;
using RFG.StateMachine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Landed State", menuName = "RFG/Platformer/Character/States/Movement State/Landed")]
    public class LandedState : State
    {
      public override Type Execute(IStateContext context)
      {
        return typeof(IdleState);
      }
    }
  }
}