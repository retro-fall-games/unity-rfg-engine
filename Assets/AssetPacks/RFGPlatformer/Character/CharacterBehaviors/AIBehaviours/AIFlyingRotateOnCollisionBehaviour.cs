using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Flying Rotate On Collision Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Flying Rotate On Collision")]
    public class AIFlyingRotateOnCollisionBehaviour : CharacterBehaviour
    {
      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.Controller.State.IsCollidingRight)
        {
          ctx.character.AIMovementState.ChangeState(typeof(AIFlyingLeftState));
        }
        else if (ctx.character.Controller.State.IsCollidingLeft)
        {
          ctx.character.AIMovementState.ChangeState(typeof(AIFlyingRightState));
        }
      }
    }
  }
}