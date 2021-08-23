using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Movement Path")]
    public class AIMovementPathBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      public float WalkSpeed = 5f;

      // public override void InitValues(CharacterBehaviour behaviour)
      // {
      //   AIMovementPathBehaviour b = (AIMovementPathBehaviour)behaviour;
      //   WalkSpeed = b.WalkSpeed;
      // }

      // public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      // {
      //   if (ctx.character.CurrentStateType != typeof(AIMovementPathState) || ctx.movementPath == null)
      //   {
      //     ctx.character.ChangeState(typeof(AIIdleState));
      //     ctx.character.ChangeState(typeof(IdleState));
      //     return;
      //   }

      //   ctx.character.Controller.RotateTowards(ctx.movementPath.NextPath);
      //   if (!ctx.movementPath.autoMove)
      //   {
      //     ctx.movementPath.Move();
      //     ctx.movementPath.CheckPath();
      //   }

      //   if (ctx.movementPath.state == MovementPath.State.OneWay && ctx.movementPath.ReachedEnd)
      //   {
      //     ctx.character.ChangeState(typeof(AIIdleState));
      //     ctx.character.ChangeState(typeof(IdleState));
      //   }
      //   else
      //   {
      //     ctx.character.ChangeState(typeof(WalkingState));
      //   }

      // }

    }
  }
}