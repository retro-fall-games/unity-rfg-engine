using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Jump")]
    public class JumpAbility : CharacterAbility
    {
      public enum JumpRestrictions
      {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
      }

      [Header("Effects")]
      public string[] JumpEffects;
      public string[] LandEffects;

      [Header("Jump Parameters")]
      public float JumpHeight = 12f;
      public float OneWayPlatformFallVelocity = -10f;

      [Header("Jump Restrictions")]
      public JumpRestrictions Restrictions;
      public int NumberOfJumps = 1;
      public int NumberOfJumpsLeft { get { return _numberOfJumpsLeft; } }
      public bool CanJumpDownOneWayPlatforms = true;

      [Header("Proportional Jumps")]
      public bool JumpIsProportionalToThePressTime = true;
      public float JumpMinAirTime = 0.1f;
      public float JumpReleaseForceFactor = 2f;

      [HideInInspector]
      private int _numberOfJumpsLeft = 0;
      private float _lastJumpTime = 0f;

      public override void Init(CharacterAbilityController.AbilityContext ctx)
      {
        NumberOfJumps = 1;
      }

      public override void Process(CharacterAbilityController.AbilityContext ctx)
      {
        if (ctx.character.Controller.State.JustGotGrounded)
        {
          ctx.character.Controller.State.IsFalling = false;
          ctx.character.Controller.State.IsJumping = false;
          ctx.transform.SpawnFromPool("Effects", LandEffects);
          ctx.character.CharacterMovementState.ChangeState(typeof(LandedState));
          _numberOfJumpsLeft = NumberOfJumps;
        }
      }

      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        JumpStart(ctx);
      }

      public override void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        JumpStop(ctx);
      }

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }

      public void JumpStart(CharacterAbilityController.AbilityContext ctx)
      {
        if (!CanJump(ctx))
        {
          return;
        }

        ctx.transform.SpawnFromPool("Effects", JumpEffects);

        ctx.character.Controller.CollisionsOnStairs(true);

        Vector2 movementVector = ctx.input.PrimaryMovement;
        float _verticalInput = movementVector.y;

        if (_verticalInput < 0f)
        {
          _lastJumpTime = Time.time;
          ctx.character.Controller.State.IsFalling = true;
          ctx.character.Controller.State.IsJumping = false;
          ctx.character.CharacterMovementState.ChangeState(typeof(FallingState));
          ctx.character.Controller.IgnoreOneWayPlatformsThisFrame = true;
          ctx.character.Controller.SetVerticalForce(OneWayPlatformFallVelocity);
          ctx.character.Controller.IgnoreStairsForTime(0.1f);
        }
        else
        {
          _lastJumpTime = Time.time;
          ctx.character.Controller.State.IsFalling = false;
          ctx.character.Controller.State.IsJumping = true;
          ctx.character.CharacterMovementState.ChangeState(typeof(JumpingState));
          _numberOfJumpsLeft--;
          ctx.character.Controller.AddVerticalForce(Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(ctx.character.Controller.Parameters.Gravity)));
        }

      }

      private void JumpStop(CharacterAbilityController.AbilityContext ctx)
      {
        if (JumpIsProportionalToThePressTime)
        {
          bool hasMinAirTime = Time.time - _lastJumpTime >= JumpMinAirTime;
          bool speedGreaterThanGravity = ctx.character.Controller.Velocity.y > Mathf.Sqrt(Mathf.Abs(ctx.character.Controller.Parameters.Gravity));
          if (hasMinAirTime && speedGreaterThanGravity)
          {
            _lastJumpTime = 0f;
            if (JumpReleaseForceFactor == 0f)
            {
              ctx.character.Controller.SetVerticalForce(0f);
            }
            else
            {
              ctx.character.Controller.AddVerticalForce(-ctx.character.Controller.Velocity.y / JumpReleaseForceFactor);
            }
          }
        }
        ctx.character.Controller.State.IsFalling = true;
        ctx.character.Controller.State.IsJumping = false;
        ctx.character.CharacterMovementState.ChangeState(typeof(FallingState));
      }

      private bool CanJump(CharacterAbilityController.AbilityContext ctx)
      {
        if (Restrictions == JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }

        if (Restrictions == JumpRestrictions.CanJumpOnGround && _numberOfJumpsLeft <= 0)
        {
          return false;
        }

        if (ctx.character.CharacterMovementState.CurrentStateType == typeof(WallClingingState))
        {
          return false;
        }

        return true;
      }

      public void SetNumberOfJumpsLeft(int numberLeft)
      {
        _numberOfJumpsLeft = numberLeft;
      }

    }
  }
}