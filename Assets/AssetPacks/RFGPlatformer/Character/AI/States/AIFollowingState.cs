using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Following State", menuName = "RFG/Platformer/Character/AI State/Following")]
    public class AIFollowingState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        if (ctx.JustRotated)
        {
          if (Time.time - ctx.LastTimeRotated < ctx.RotateSpeed)
          {
            ctx.controller.SetHorizontalForce(0);
            ctx.controller.SetVerticalForce(0);
            return null;
          }
        }

        FollowTarget(ctx);
        return null;
      }

      private void FollowTarget(AIBrainBehaviour.AIStateContext ctx)
      {
        // Rotate to always face the target
        if (ctx.aggro.target2 != null)
        {
          bool didRotate = ctx.controller.RotateTowards(ctx.aggro.target2);
          if (didRotate && ctx.RotateSpeed > 0)
          {
            ctx.JustRotated = true;
            ctx.LastTimeRotated = Time.time;
            ctx.controller.SetHorizontalForce(0);
            ctx.controller.SetVerticalForce(0);
            return;
          }
        }

        // Move towards that target
        float normalizedHorizontalSpeed = 0f;

        if (ctx.controller.State.IsFacingRight)
        {
          normalizedHorizontalSpeed = 1f;
        }
        else
        {
          normalizedHorizontalSpeed = -1f;
        }

        ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
        ctx.animator.Play(ctx.aiBrain.RunningSettings.RunningClip);

        float movementFactor = ctx.controller.Parameters.GroundSpeedFactor;
        float movementSpeed = normalizedHorizontalSpeed * ctx.aiBrain.RunningSettings.RunningSpeed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);

        if (ctx.aiBrain.CanFollowVertically)
        {
          float normalizedVerticalSpeed = 0f;
          if (ctx.aggro.target2.transform.position.y > ctx.transform.position.y)
          {
            normalizedVerticalSpeed = 1f;
          }
          else
          {
            normalizedVerticalSpeed = -1f;
          }
          float airMovementFactor = ctx.controller.Parameters.AirSpeedFactor;
          float airMovementSpeed = normalizedVerticalSpeed * ctx.aiBrain.RunningSettings.RunningSpeed * ctx.controller.Parameters.SpeedFactor;
          float verticalMovementForce = Mathf.Lerp(ctx.controller.Speed.y, airMovementSpeed, Time.deltaTime * airMovementFactor);
          ctx.controller.SetVerticalForce(verticalMovementForce);
        }
      }
    }
  }
}