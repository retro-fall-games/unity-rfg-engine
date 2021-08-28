using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Attacking State", menuName = "RFG/Platformer/Character/AI State/Attacking")]
    public class AIAttackingState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        if (ctx.JustRotated)
        {
          if (Time.time - ctx.LastTimeRotated < ctx.RotateSpeed)
          {
            ctx.controller.SetHorizontalForce(0);
            return null;
          }
        }

        FollowTarget(ctx);
        Attack(ctx);
        return null;
      }

      private void FollowTarget(AIBrainBehaviour.AIStateContext ctx)
      {
        // Rotate to always face the target
        if (ctx.aggro.target2 != null)
        {
          bool didRotate = ctx.controller.RotateTowards(ctx.aggro.target2);
          if (didRotate)
          {
            ctx.JustRotated = true;
            ctx.LastTimeRotated = Time.time;
            ctx.controller.SetHorizontalForce(0);
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

        bool useRunning = !ctx.RunningCooldown;

        float speed = useRunning ? ctx.aiBrain.RunningSettings.RunningSpeed : ctx.aiBrain.WalkingSettings.WalkingSpeed;

        if (useRunning)
        {

          ctx.RunningPower -= ctx.aiBrain.RunningSettings.PowerGainPerFrame;
          if (ctx.RunningPower <= 0)
          {
            ctx.RunningCooldown = true;
            speed = ctx.aiBrain.WalkingSettings.WalkingSpeed;
            ctx.LastTimeRunningCooldown = Time.time;
          }
          else
          {
            ctx.transform.DeactivatePoolByTag("Effects", ctx.aiBrain.WalkingSettings.WalkingEffects);
            ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
            ctx.animator.Play(ctx.aiBrain.RunningSettings.RunningClip);
          }
        }
        else
        {
          ctx.transform.DeactivatePoolByTag("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
          ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.WalkingSettings.WalkingEffects);
          ctx.animator.Play(ctx.aiBrain.WalkingSettings.WalkingClip);
          if (Time.time - ctx.LastTimeRunningCooldown > ctx.aiBrain.RunningSettings.CooldownTimer)
          {
            ctx.RunningPower += ctx.aiBrain.RunningSettings.PowerGainPerFrame;
            if (ctx.RunningPower >= ctx.aiBrain.RunningSettings.RunningPower)
            {
              ctx.RunningPower = ctx.aiBrain.RunningSettings.RunningPower;
              ctx.RunningCooldown = false;
            }
          }
        }

        float movementFactor = ctx.controller.Parameters.GroundSpeedFactor;
        float movementSpeed = normalizedHorizontalSpeed * speed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);
      }

      private void Attack(AIBrainBehaviour.AIStateContext ctx)
      {
        //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
        //   {
        //     ctx.equipmentSet.PrimaryWeapon?.Perform();
        //   }

        //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
        //   {
        //     ctx.equipmentSet.SecondaryWeapon?.Perform();
        //   }
      }
    }
  }
}