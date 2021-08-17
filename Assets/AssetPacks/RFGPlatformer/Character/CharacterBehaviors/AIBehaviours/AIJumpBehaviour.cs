using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Jump Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Jump")]
    public class AIJumpBehaviour : CharacterBehaviour
    {
      public enum JumpRestrictions
      {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
      }

      [Header("Jump Parameters")]
      public float JumpHeight = 12f;
      public float HorizontalSpeed = 3f;

      [Header("Jump Restrictions")]
      public JumpRestrictions Restrictions;

      [Header("Effects")]
      public string[] JumpEffects;
      public string[] LandEffects;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIJumpBehaviour b = (AIJumpBehaviour)behaviour;
        JumpHeight = b.JumpHeight;
        HorizontalSpeed = b.HorizontalSpeed;
        Restrictions = b.Restrictions;
        JumpEffects = b.JumpEffects;
        LandEffects = b.LandEffects;
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.Controller.State.JustGotGrounded)
        {
          ctx.transform.SpawnFromPool("Effects", LandEffects);
        }
        if (ctx.character.AIMovementState.CurrentStateType == typeof(AIJumpingLeftState) || ctx.character.AIMovementState.CurrentStateType == typeof(AIJumpingRightState))
        {
          JumpStart(ctx);
        }
      }

      public void JumpStart(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (!CanJump(ctx))
        {
          return;
        }

        ctx.transform.SpawnFromPool("Effects", JumpEffects);

        if (ctx.character.AIMovementState.CurrentStateType == typeof(AIJumpingLeftState) && ctx.character.Controller.State.IsFacingRight)
        {
          ctx.character.Controller.Flip();
        }
        else if (ctx.character.AIMovementState.CurrentStateType == typeof(AIJumpingRightState) && !ctx.character.Controller.State.IsFacingRight)
        {
          ctx.character.Controller.Flip();
        }

        // Jump
        ctx.character.Controller.CollisionsOnStairs(true);
        ctx.character.Controller.State.IsFalling = false;
        ctx.character.Controller.State.IsJumping = true;
        ctx.character.CharacterMovementState.ChangeState(typeof(JumpingState));
        ctx.character.Controller.AddVerticalForce(Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(ctx.character.Controller.Parameters.Gravity)));

        // Move horizontally
        float _normalizedHorizontalSpeed = 0f;
        if (ctx.character.Controller.State.IsFacingRight)
        {
          _normalizedHorizontalSpeed = 1f;
        }
        else
        {
          _normalizedHorizontalSpeed = -1f;
        }

        float movementFactor = ctx.character.Controller.Parameters.AirSpeedFactor;
        float movementSpeed = _normalizedHorizontalSpeed * HorizontalSpeed * ctx.character.Controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);

        JumpStop(ctx);
      }

      private void JumpStop(CharacterBehaviourController.BehaviourContext ctx)
      {
        ctx.character.Controller.State.IsFalling = true;
        ctx.character.CharacterMovementState.ChangeState(typeof(FallingState));
        ctx.character.AIMovementState.RestorePreviousState();
      }

      private bool CanJump(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (Restrictions == JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }
        if (Restrictions == JumpRestrictions.CanJumpOnGround && ctx.character.Controller.State.IsGrounded)
        {
          return true;
        }
        return false;
      }

    }
  }
}