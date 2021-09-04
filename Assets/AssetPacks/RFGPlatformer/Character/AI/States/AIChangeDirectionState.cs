using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Change Direction State", menuName = "RFG/Platformer/Character/AI State/Change Direction")]
    public class AIChangeDirectionState : AIState
    {
      public override Type Execute(AIStateContext ctx)
      {
        ctx.controller.Flip();
        ctx.aiState.RestorePreviousDecision();
        return null;
      }
    }
  }
}