using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Aggro Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Aggro")]
    public class AIAggroBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float RunSpeed = 5f;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIAggroBehaviour b = (AIAggroBehaviour)behaviour;
        RunSpeed = b.RunSpeed;
      }

      public override void EarlyProcess(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.aggro.HasAggro)
        {
          ctx.character.AIState.ChangeState(typeof(AIAttackingState));
        }
        else
        {
          ctx.character.AIState.ChangeState(typeof(AIWanderingState));
        }
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType == typeof(AIAttackingState))
        {
          // Rotate to always face the target
          ctx.character.Controller.RotateTowards(ctx.aggro.target2);

          // Move towards that target
          float _normalizedHorizontalSpeed = 0f;

          if (ctx.character.Controller.State.IsFacingRight)
          {
            ctx.character.AIMovementState.ChangeState(typeof(AIRunningRightState));
            _normalizedHorizontalSpeed = 1f;
          }
          else
          {
            ctx.character.AIMovementState.ChangeState(typeof(AIRunningLeftState));
            _normalizedHorizontalSpeed = -1f;
          }

          ctx.character.CharacterMovementState.ChangeState(typeof(RunningState));

          float movementFactor = ctx.character.Controller.State.IsGrounded ? ctx.character.Controller.Parameters.GroundSpeedFactor : ctx.character.Controller.Parameters.AirSpeedFactor;
          float movementSpeed = _normalizedHorizontalSpeed * RunSpeed * ctx.character.Controller.Parameters.SpeedFactor;
          float horizontalMovementForce = Mathf.Lerp(ctx.character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

          ctx.character.Controller.SetHorizontalForce(horizontalMovementForce);
        }
      }
    }
  }
}