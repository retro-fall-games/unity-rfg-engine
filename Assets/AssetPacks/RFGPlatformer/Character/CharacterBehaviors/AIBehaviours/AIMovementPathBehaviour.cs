using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Movement Path Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Movement Path")]
    public class AIMovementPathBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float WalkSpeed = 5f;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIMovementPathBehaviour b = (AIMovementPathBehaviour)behaviour;
        WalkSpeed = b.WalkSpeed;
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType != typeof(AIMovementPathState) || ctx.movementPath == null)
        {
          ctx.character.AIState.ChangeState(typeof(AIIdleState));
          ctx.character.CharacterMovementState.ChangeState(typeof(IdleState));
          return;
        }

        ctx.character.Controller.RotateTowards(ctx.movementPath.NextPath);
        if (!ctx.movementPath.autoMove)
        {
          ctx.movementPath.Move();
          ctx.movementPath.CheckPath();
        }

        if (ctx.movementPath.state == MovementPath.State.OneWay && ctx.movementPath.ReachedEnd)
        {
          ctx.character.AIState.ChangeState(typeof(AIIdleState));
          ctx.character.CharacterMovementState.ChangeState(typeof(IdleState));
        }
        else
        {
          ctx.character.CharacterMovementState.ChangeState(typeof(WalkingState));
        }

      }

    }
  }
}