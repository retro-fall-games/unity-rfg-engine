using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Wall Jump")]
    public class WallJumpAbility : CharacterAbility
    {
      [Header("Settings")]
      public float Threshold = 0.01f;
      public Vector2 WallJumpForce = new Vector2(10f, 4f);

      [Header("Effect")]
      public string[] JumpEffects;

      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(WallClingingState))
        {
          WallJump(ctx);
        }
      }

      private void WallJump(CharacterAbilityController.AbilityContext ctx)
      {
        ctx.transform.SpawnFromPool("Effects", JumpEffects);
        ctx.character.CharacterMovementState.ChangeState(typeof(WallJumpingState));
        ctx.character.Controller.SlowFall(0f);

        Vector2 _movementVector = ctx.input.PrimaryMovement;
        float _horizontalInput = _movementVector.x;
        bool isClingingLeft = ctx.character.Controller.State.IsCollidingLeft && _horizontalInput <= -Threshold;
        bool isClingingRight = ctx.character.Controller.State.IsCollidingRight && _horizontalInput >= Threshold;

        float wallJumpDirection;
        if (isClingingRight)
        {
          wallJumpDirection = -1f;
        }
        else
        {
          wallJumpDirection = 1f;
        }

        Vector2 wallJumpVector = new Vector2(wallJumpDirection * WallJumpForce.x, Mathf.Sqrt(2f * WallJumpForce.y * Mathf.Abs(ctx.character.Controller.Parameters.Gravity)));

        ctx.character.Controller.AddForce(wallJumpVector);
      }

    }
  }
}