using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Horizontal Movement Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Horizontal Movement")]
    public class HorizontalMovementAbility : CharacterAbility
    {
      [Header("Settings")]
      public float Speed = 5f;

      [Header("Sound Effect")]
      public string[] MovementEffects;

      public override void Process(CharacterAbilityController.AbilityContext ctx)
      {
        Vector2 movementVector = ctx.input.PrimaryMovement;
        float horizontalSpeed = movementVector.x;
        // float _verticalInput = inputMovement.y;

        if (horizontalSpeed > 0f)
        {
          if (!ctx.character.Controller.State.IsFacingRight && !ctx.character.Controller.rotateOnMouseCursor)
          {
            ctx.character.Controller.Flip();
          }
        }
        else if (horizontalSpeed < 0f)
        {
          if (ctx.character.Controller.State.IsFacingRight && !ctx.character.Controller.rotateOnMouseCursor)
          {
            ctx.character.Controller.Flip();
          }
        }

        // If the movement state is dashing return so it wont get set back to idle
        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          return;
        }

        // Call the Use method to call any SoundFx
        if (horizontalSpeed != 0f)
        {
          ctx.transform.SpawnFromPool("Effects", MovementEffects);
        }

        // if (_verticalInput >= 1 || _verticalInput <= -1)
        // {
        //   ctx.character.Controller.CollisionsOnStairs(true);
        // }

        if ((!ctx.character.Controller.State.IsJumping && !ctx.character.Controller.State.IsFalling && ctx.character.Controller.State.IsGrounded) || ctx.character.Controller.State.JustGotGrounded)
        {
          ctx.character.CharacterMovementState.ChangeState(horizontalSpeed == 0 ? typeof(IdleState) : typeof(WalkingState));
        }

        float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
        float movementSpeed = horizontalSpeed * Speed * ctx.character.Controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);
      }

    }
  }
}