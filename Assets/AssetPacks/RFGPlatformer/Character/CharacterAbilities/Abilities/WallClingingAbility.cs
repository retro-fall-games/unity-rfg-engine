using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Clinging Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Wall Clinging")]
    public class WallClingingAbility : CharacterAbility
    {
      [Header("Settings")]
      [Range(0.01f, 1f)]
      public float WallClingingSlowFactor = 0.6f;
      public float RaycastVerticalOffset = 0f;
      public float WallClingingTolerance = 0.3f;
      public float Threshold = 0.1f;

      [Header("Effects")]
      public string[] ClingEffects;

      public override void Process(CharacterAbilityController.AbilityContext ctx)
      {
        if (ctx.character.Controller.State.IsGrounded || ctx.character.Controller.Velocity.y >= 0)
        {
          ctx.character.Controller.SlowFall(0f);
          return;
        }

        Vector2 _movementVector = ctx.input.PrimaryMovement;

        float _horizontalInput = _movementVector.x;
        float _verticalInput = _movementVector.y;

        bool isClingingLeft = ctx.character.Controller.State.IsCollidingLeft && _horizontalInput <= -Threshold;
        bool isClingingRight = ctx.character.Controller.State.IsCollidingRight && _horizontalInput >= Threshold;

        // If we are wall clinging, then change the state
        if (isClingingLeft || isClingingRight)
        {
          // Slow the fall speed
          ctx.character.Controller.SlowFall(WallClingingSlowFactor);
          ctx.character.CharacterMovementState.ChangeState(typeof(WallClingingState));
        }

        // If we are in a wall clinging state then make sure we are still wall clinging
        // if not then go back to idle
        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(WallClingingState))
        {
          bool shouldExit = false;
          if (ctx.character.Controller.State.IsGrounded || ctx.character.Controller.Velocity.y >= 0)
          {
            // If the character is grounded or moving up
            shouldExit = true;
          }

          Vector3 raycastOrigin = ctx.transform.position;
          Vector3 raycastDirection;
          Vector3 right = ctx.transform.right;

          if (isClingingRight && !ctx.character.Controller.State.IsFacingRight)
          {
            right = -right;
          }
          else if (isClingingLeft && ctx.character.Controller.State.IsFacingRight)
          {
            right = -right;
          }

          raycastOrigin = raycastOrigin + right * ctx.character.Controller.Width() / 2 + ctx.transform.up * RaycastVerticalOffset;
          raycastDirection = right - ctx.transform.up;

          LayerMask mask = ctx.character.Controller.platformMask & (~ctx.character.Controller.oneWayPlatformMask | ~ctx.character.Controller.oneWayMovingPlatformMask);

          RaycastHit2D hit = RFG.Physics2D.Raycast(raycastOrigin, raycastDirection, WallClingingTolerance, mask, Color.red);

          if (isClingingRight)
          {
            if (!hit || _horizontalInput <= Threshold)
            {
              shouldExit = true;
            }
          }
          else
          {
            if (!hit || _horizontalInput >= -Threshold)
            {
              shouldExit = true;
            }
          }
          if (shouldExit)
          {
            ctx.character.Controller.SlowFall(0f);
            ctx.character.CharacterMovementState.ChangeState(typeof(FallingState));
          }
          else
          {
            ctx.transform.SpawnFromPool("Effects", ClingEffects);
          }
        }

        if (ctx.character.CharacterMovementState.PreviousStateType == typeof(WallClingingState) && ctx.character.CharacterMovementState.CurrentStateType != typeof(WallClingingState))
        {
          ctx.character.Controller.SlowFall(0f);
        }
      }

    }
  }
}