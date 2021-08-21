using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Flying Aggro Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Flying Aggro")]
    public class AIFlyingAggroBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float FlySpeed = 5f;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIFlyingAggroBehaviour b = (AIFlyingAggroBehaviour)behaviour;
        FlySpeed = b.FlySpeed;
      }

      public override void EarlyProcess(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.aggro.HasAggro)
        {
          ctx.character.AIState.ChangeState(typeof(AIAttackingState));
        }
        else
        {
          ctx.character.AIState.ChangeState(typeof(AIFlyingState));
        }
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType == typeof(AIAttackingState))
        {
          ctx.character.Controller.RotateTowards(ctx.aggro.target2);

          if (ctx.character.Controller.State.IsFacingRight)
          {
            ctx.character.AIMovementState.ChangeState(typeof(AIFlyingRightState));
          }
          else
          {
            ctx.character.AIMovementState.ChangeState(typeof(AIFlyingLeftState));
          }

          ctx.transform.position = Vector2.MoveTowards(ctx.transform.position, ctx.aggro.target2.transform.position, FlySpeed * ctx.character.Controller.Parameters.SpeedFactor * Time.deltaTime);
        }
      }
    }
  }
}