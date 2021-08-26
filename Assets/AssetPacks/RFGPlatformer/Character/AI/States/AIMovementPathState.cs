using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Movement Path State", menuName = "RFG/Platformer/Character/AI State/Movement Path")]
    public class AIMovementPathState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        ctx.controller.RotateTowards(ctx.movementPath.NextPath);
        if (!ctx.movementPath.autoMove)
        {
          ctx.movementPath.Move();
          ctx.movementPath.CheckPath();
        }

        if (ctx.movementPath.state == MovementPath.State.OneWay && ctx.movementPath.ReachedEnd)
        {
          return typeof(AIIdleState);
        }
        return null;
      }
    }
  }
}