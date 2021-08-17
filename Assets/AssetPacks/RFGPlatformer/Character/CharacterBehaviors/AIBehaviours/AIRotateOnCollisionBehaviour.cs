using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Rotate On Collision Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Rotate On Collision")]
    public class AIRotateOnCollisionBehaviour : CharacterBehaviour
    {
      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType != typeof(AIWanderingState))
        {
          return;
        }
        if (ctx.character.Controller.State.IsCollidingRight)
        {
          ctx.character.AIMovementState.ChangeState(typeof(AIWalkingLeftState));
        }
        else if (ctx.character.Controller.State.IsCollidingLeft)
        {
          ctx.character.AIMovementState.ChangeState(typeof(AIWalkingRightState));
        }
      }
    }
  }
}