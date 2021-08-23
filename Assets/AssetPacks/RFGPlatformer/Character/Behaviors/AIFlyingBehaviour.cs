using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Flying")]
    public class AIFlyingBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      public float FlySpeed = 5f;

      // public override void InitValues(CharacterBehaviour behaviour)
      // {
      //   AIFlyingBehaviour b = (AIFlyingBehaviour)behaviour;
      //   FlySpeed = b.FlySpeed;
      // }

      // public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      // {

      //   float _normalizedHorizontalSpeed = 0f;

      //   if (ctx.character.CurrentStateType == typeof(AIFlyingRightState))
      //   {
      //     _normalizedHorizontalSpeed = 1f;
      //     if (!ctx.character.Controller.State.IsFacingRight)
      //     {
      //       ctx.character.Controller.Flip();
      //     }
      //   }
      //   else if (ctx.character.CurrentStateType == typeof(AIFlyingLeftState))
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

      //   float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
      //   float movementSpeed = _normalizedHorizontalSpeed * FlySpeed * ctx.character.Controller.Parameters.SpeedFactor;
      //   float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

      //   ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);

      // }

    }
  }
}