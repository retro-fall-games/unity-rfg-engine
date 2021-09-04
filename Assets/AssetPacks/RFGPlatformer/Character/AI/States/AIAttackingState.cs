using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Attacking State", menuName = "RFG/Platformer/Character/AI State/Attacking")]
    public class AIAttackingState : AIState
    {
      public override Type Execute(AIStateContext ctx)
      {
        if (ctx.JustRotated())
        {
          return null;
        }
        if (ctx.RotateTowards())
        {
          return null;
        }
        if (ctx.PauseOnDangle())
        {
          return null;
        }
        float speed = ctx.WalkOrRun();
        ctx.MoveHorizontally(speed);
        ctx.MoveVertically(speed);
        ctx.Attack();
        return null;
      }
    }
  }
}