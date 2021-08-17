using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Wandering Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Wandering")]
    public class AIWanderingBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float WalkSpeed = 5f;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIWanderingBehaviour b = (AIWanderingBehaviour)behaviour;
        WalkSpeed = b.WalkSpeed;
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType != typeof(AIWanderingState))
        {
          return;
        }

        float _normalizedHorizontalSpeed = 0f;

        if (ctx.character.AIMovementState.CurrentStateType == typeof(AIWalkingRightState))
        {
          _normalizedHorizontalSpeed = 1f;
          if (!ctx.character.Controller.State.IsFacingRight)
          {
            ctx.character.Controller.Flip();
          }
        }
        else if (ctx.character.AIMovementState.CurrentStateType == typeof(AIWalkingLeftState))
        {
          _normalizedHorizontalSpeed = -1f;
          if (ctx.character.Controller.State.IsFacingRight)
          {
            ctx.character.Controller.Flip();
          }
        }
        else if (ctx.character.AIMovementState.CurrentStateType == typeof(AIIdleState))
        {
          _normalizedHorizontalSpeed = 0f;
        }

        ctx.character.CharacterMovementState.ChangeState(_normalizedHorizontalSpeed == 0 ? typeof(IdleState) : typeof(WalkingState));

        float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
        float movementSpeed = _normalizedHorizontalSpeed * WalkSpeed * ctx.character.Controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);

      }

    }
  }
}