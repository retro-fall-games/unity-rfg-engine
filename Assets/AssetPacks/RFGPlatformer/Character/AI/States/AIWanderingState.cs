using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Wandering State", menuName = "RFG/Platformer/Character/AI State/Wandering")]
    public class AIWanderingState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        if (ctx.controller.State.IsCollidingLeft || ctx.controller.State.IsCollidingRight)
        {
          ctx.controller.Flip();
        }

        float _normalizedHorizontalSpeed = 0f;

        if (ctx.controller.State.IsFacingRight)
        {
          _normalizedHorizontalSpeed = 1f;
        }
        else
        {
          _normalizedHorizontalSpeed = -1f;
        }

        ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.WalkingSettings.WalkingEffects);
        ctx.animator.Play(ctx.aiBrain.WalkingSettings.WalkingClip);

        float movementFactor = ctx.controller.Parameters.GroundSpeedFactor;
        float movementSpeed = _normalizedHorizontalSpeed * ctx.aiBrain.WalkingSettings.WalkingSpeed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);
        return null;
      }
    }
  }
}