using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Aggro")]
    public class AIAggroBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      public float RunSpeed = 5f;

      // public override void InitValues(CharacterBehaviour behaviour)
      // {
      //   AIAggroBehaviour b = (AIAggroBehaviour)behaviour;
      //   RunSpeed = b.RunSpeed;
      // }

      public void EarlyProcess()
      {
        // if (ctx.aggro.HasAggro)
        // {
        //   //ctx.character.ChangeState(typeof(AIAttackingState));
        // }
        // else
        // {
        //   //ctx.character.ChangeState(typeof(AIWanderingState));
        // }
      }

      public void Process()
      {
        // if (ctx.character.CurrentStateType == typeof(AIAttackingState))
        // {
        //   // Rotate to always face the target
        //   ctx.character.Controller.RotateTowards(ctx.aggro.target2);

        //   // Move towards that target
        //   float _normalizedHorizontalSpeed = 0f;

        //   if (ctx.character.Controller.State.IsFacingRight)
        //   {
        //     ctx.character.ChangeState(typeof(AIRunningRightState));
        //     _normalizedHorizontalSpeed = 1f;
        //   }
        //   else
        //   {
        //     ctx.character.ChangeState(typeof(AIRunningLeftState));
        //     _normalizedHorizontalSpeed = -1f;
        //   }

        //   ctx.character.ChangeState(typeof(RunningState));

        //   float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
        //   float movementSpeed = _normalizedHorizontalSpeed * RunSpeed * ctx.character.Controller.Parameters.SpeedFactor;
        //   float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        //   ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);
        // }
      }
    }
  }
}