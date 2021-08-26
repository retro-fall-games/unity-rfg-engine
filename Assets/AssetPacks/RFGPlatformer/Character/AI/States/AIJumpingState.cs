using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Jumping State", menuName = "RFG/Platformer/Character/AI State/Jumping")]
    public class AIJumpingState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        if (ctx.controller.State.JustGotGrounded)
        {
          ctx.controller.SetHorizontalForce(0);
          ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.JumpSettings.LandEffects);
          ctx.aiState.RestorePreviousDecision();
        }
        if (ctx.controller.State.IsGrounded)
        {
          JumpStart(ctx);
        }
        return null;
      }

      public void JumpStart(AIBrainBehaviour.AIStateContext ctx)
      {
        if (!CanJump(ctx))
        {
          return;
        }

        ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.JumpSettings.JumpEffects);
        ctx.animator.Play(ctx.aiBrain.JumpSettings.JumpingClip);

        // Jump
        ctx.controller.CollisionsOnStairs(true);
        ctx.controller.State.IsFalling = false;
        ctx.controller.State.IsJumping = true;
        ctx.controller.AddVerticalForce(Mathf.Sqrt(2f * ctx.aiBrain.JumpSettings.JumpHeight * Mathf.Abs(ctx.controller.Parameters.Gravity)));

        // Move horizontally
        float normalizedHorizontalSpeed = 0f;
        if (ctx.controller.State.IsFacingRight)
        {
          normalizedHorizontalSpeed = 1f;
        }
        else
        {
          normalizedHorizontalSpeed = -1f;
        }

        float speed = ctx.aiBrain.WalkingSettings.WalkingSpeed;
        if (ctx.aggro != null && ctx.aggro.HasAggro)
        {
          speed = ctx.aiBrain.RunningSettings.RunningSpeed;
        }

        float movementFactor = ctx.controller.Parameters.AirSpeedFactor;
        float movementSpeed = normalizedHorizontalSpeed * speed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);

        JumpStop(ctx);
      }

      private void JumpStop(AIBrainBehaviour.AIStateContext ctx)
      {
        ctx.controller.State.IsFalling = true;
        ctx.controller.State.IsJumping = false;
        ctx.aiState.RestorePreviousDecision();
      }

      private bool CanJump(AIBrainBehaviour.AIStateContext ctx)
      {
        if (ctx.aiBrain.JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }
        if (ctx.aiBrain.JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpOnGround && ctx.controller.State.IsGrounded)
        {
          return true;
        }
        return false;
      }
    }
  }
}