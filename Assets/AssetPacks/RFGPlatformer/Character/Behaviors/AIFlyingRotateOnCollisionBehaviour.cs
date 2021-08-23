using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Flying Rotate On Collision")]
    public class AIFlyingRotateOnCollisionBehaviour : MonoBehaviour
    {
      // public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      // {
      //   if (ctx.character.Controller.State.IsCollidingRight)
      //   {
      //     ctx.character.ChangeState(typeof(AIFlyingLeftState));
      //   }
      //   else if (ctx.character.Controller.State.IsCollidingLeft)
      //   {
      //     ctx.character.ChangeState(typeof(AIFlyingRightState));
      //   }
      // }
    }
  }
}