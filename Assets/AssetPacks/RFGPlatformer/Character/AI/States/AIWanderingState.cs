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
        if (ctx.characterContext.settingsPack == null || ctx.characterContext.settingsPack.WalkingSettings == null)
          return null;

        WalkingSettings WalkingSettings = ctx.characterContext.settingsPack.WalkingSettings;
        ctx.FlipOnCollision();
        ctx.FlipOnDangle();
        ctx.controller.State.IsWalking = true;
        ctx.MoveHorizontally(WalkingSettings.WalkingSpeed);
        ctx.TouchingWalls();
        return null;
      }

    }
  }
}