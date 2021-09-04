using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public static class AIStateContextEx
    {
      public static bool JustRotated(this AIStateContext ctx)
      {
        if (ctx.JustRotated)
        {
          if (Time.time - ctx.LastTimeRotated < ctx.RotateSpeed)
          {
            ctx.controller.SetHorizontalForce(0);
            ctx.controller.SetVerticalForce(0);
            return true;
          }
        }
        return false;
      }

      public static bool RotateTowards(this AIStateContext ctx)
      {
        if (ctx.aggro.target2 != null)
        {
          bool didRotate = ctx.controller.RotateTowards(ctx.aggro.target2);
          if (didRotate && ctx.RotateSpeed > 0)
          {
            ctx.JustRotated = true;
            ctx.LastTimeRotated = Time.time;
            ctx.controller.SetHorizontalForce(0);
            ctx.controller.SetVerticalForce(0);
            return true;
          }
        }
        return false;
      }

      public static bool TouchingWalls(this AIStateContext ctx)
      {
        if ((ctx.controller.State.IsWalking || ctx.controller.State.IsRunning))
        {
          if ((ctx.controller.State.IsCollidingLeft) || (ctx.controller.State.IsCollidingRight))
          {
            ctx.controller.State.IsWalking = false;
            ctx.controller.State.IsRunning = false;
            return true;
          }
        }
        return false;
      }

      public static void FlipOnCollision(this AIStateContext ctx)
      {
        if (ctx.controller.State.IsCollidingLeft || ctx.controller.State.IsCollidingRight)
        {
          ctx.controller.Flip();
        }
      }

      public static bool IsDangling(this AIStateContext ctx)
      {
        Vector3 raycastOrigin = Vector3.zero;
        if (ctx.controller.State.IsFacingRight)
        {
          raycastOrigin = ctx.transform.position + ctx.aiBrain.DanglingSettings.DanglingRaycastOrigin.x * Vector3.right + ctx.aiBrain.DanglingSettings.DanglingRaycastOrigin.y * ctx.transform.up;
        }
        else
        {
          raycastOrigin = ctx.transform.position - ctx.aiBrain.DanglingSettings.DanglingRaycastOrigin.x * Vector3.right + ctx.aiBrain.DanglingSettings.DanglingRaycastOrigin.y * ctx.transform.up;
        }

        RaycastHit2D hit = RFG.Physics2D.RayCast(raycastOrigin, -ctx.transform.up, ctx.aiBrain.DanglingSettings.DanglingRaycastLength, ctx.controller.PlatformMask | ctx.controller.OneWayPlatformMask | ctx.controller.OneWayMovingPlatformMask, Color.gray, true);

        if (!hit)
        {
          return true;
        }
        return false;
      }

      public static void FlipOnDangle(this AIStateContext ctx)
      {
        if (ctx.aiBrain.DanglingSettings != null && ctx.IsDangling())
        {
          ctx.controller.Flip();
        }
      }

      public static bool PauseOnDangle(this AIStateContext ctx)
      {
        if (ctx.aiBrain.DanglingSettings != null && ctx.IsDangling())
        {
          ctx.MoveHorizontally(0);
          return true;
        }
        return false;
      }

      public static void MoveHorizontally(this AIStateContext ctx, float speed)
      {
        float _normalizedHorizontalSpeed = 0f;

        if (ctx.controller.State.IsFacingRight)
        {
          _normalizedHorizontalSpeed = 1f;
        }
        else
        {
          _normalizedHorizontalSpeed = -1f;
        }

        float movementFactor = ctx.controller.Parameters.GroundSpeedFactor;
        float movementSpeed = _normalizedHorizontalSpeed * speed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);
      }

      public static void MoveVertically(this AIStateContext ctx, float speed)
      {
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
          float airMovementSpeed = normalizedVerticalSpeed * speed * ctx.controller.Parameters.SpeedFactor;
          float verticalMovementForce = Mathf.Lerp(ctx.controller.Speed.y, airMovementSpeed, Time.deltaTime * airMovementFactor);
          ctx.controller.SetVerticalForce(verticalMovementForce);
        }
      }

      public static void Attack(this AIStateContext ctx)
      {
        if (ctx.equipmentSet == null)
        {
          return;
        }
        int decisionIndex = UnityEngine.Random.Range(0, 100);
        if (decisionIndex < 900)
        {
          ctx.equipmentSet.PrimaryWeapon?.Perform();
        }
        else
        {
          ctx.equipmentSet.SecondaryWeapon?.Perform();
        }
      }

      public static float WalkOrRun(this AIStateContext ctx)
      {
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
            ctx.controller.State.IsRunning = false;
            ctx.controller.State.IsWalking = true;
          }
          else
          {
            ctx.transform.DeactivatePoolByTag("Effects", ctx.aiBrain.WalkingSettings.WalkingEffects);
            ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
            ctx.animator.Play(ctx.aiBrain.RunningSettings.RunningClip);
            ctx.controller.State.IsRunning = true;
            ctx.controller.State.IsWalking = false;
          }
        }
        else
        {
          ctx.transform.DeactivatePoolByTag("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
          ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.WalkingSettings.WalkingEffects);
          ctx.animator.Play(ctx.aiBrain.WalkingSettings.WalkingClip);
          ctx.controller.State.IsWalking = true;
          ctx.controller.State.IsRunning = false;
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
        return speed;
      }

    }
  }
}