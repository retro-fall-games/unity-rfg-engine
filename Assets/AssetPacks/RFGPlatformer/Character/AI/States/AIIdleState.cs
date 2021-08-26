using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Idle State", menuName = "RFG/Platformer/Character/AI State/Idle")]
    public class AIIdleState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        ctx.controller.SetHorizontalForce(0);
        return null;
      }
    }
  }
}