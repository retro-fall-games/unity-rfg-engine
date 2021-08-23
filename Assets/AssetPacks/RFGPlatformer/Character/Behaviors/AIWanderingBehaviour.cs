using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Wandering")]
    public class AIWanderingBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      public float WalkSpeed = 5f;

      // public override void InitValues(CharacterBehaviour behaviour)
      // {
      //   AIWanderingBehaviour b = (AIWanderingBehaviour)behaviour;
      //   WalkSpeed = b.WalkSpeed;
      // }

      // public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      // {
      //   if (ctx.character.CurrentStateType != typeof(AIWanderingState))
      //   {
      //     return;
      //   }

      //   float _normalizedHorizontalSpeed = 0f;

      //   if (ctx.character.CurrentStateType == typeof(AIWalkingRightState))
      //   {
      //     _normalizedHorizontalSpeed = 1f;
      //     if (!ctx.character.Controller.State.IsFacingRight)
      //     {
      //       ctx.character.Controller.Flip();
      //     }
      //   }
      //   else if (ctx.character.CurrentStateType == typeof(AIWalkingLeftState))
      //   {
      //     _normalizedHorizontalSpeed = -1f;
      //     if (ctx.character.Controller.State.IsFacingRight)
      //     {
      //       ctx.character.Controller.Flip();
      //     }
      //   }
      //   else if (ctx.character.CurrentStateType == typeof(AIIdleState))
      //   {
      //     _normalizedHorizontalSpeed = 0f;
      //   }

      //   ctx.character.ChangeState(_normalizedHorizontalSpeed == 0 ? typeof(IdleState) : typeof(WalkingState));

      //   float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
      //   float movementSpeed = _normalizedHorizontalSpeed * WalkSpeed * ctx.character.Controller.Parameters.SpeedFactor;
      //   float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

      //   ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);

      // }

    }
  }
}