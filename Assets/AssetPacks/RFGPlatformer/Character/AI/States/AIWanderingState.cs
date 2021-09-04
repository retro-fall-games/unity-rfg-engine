using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Wandering State", menuName = "RFG/Platformer/Character/AI State/Wandering")]
    public class AIWanderingState : AIState
    {
      public override Type Execute(AIStateContext ctx)
      {
        ctx.FlipOnCollision();
        ctx.FlipOnDangle();
        ctx.controller.State.IsWalking = true;
        ctx.MoveHorizontally(ctx.aiBrain.WalkingSettings.WalkingSpeed);
        ctx.TouchingWalls();
        return null;
      }

    }
  }
}