using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Rotate On Collision")]
    public class AIRotateOnCollisionBehaviour : MonoBehaviour
    {
      // public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      // {
      //   if (ctx.character.CurrentStateType != typeof(AIWanderingState))
      //   {
      //     return;
      //   }
      //   if (ctx.character.Controller.State.IsCollidingRight)
      //   {
      //     ctx.character.ChangeState(typeof(AIWalkingLeftState));
      //   }
      //   else if (ctx.character.Controller.State.IsCollidingLeft)
      //   {
      //     ctx.character.ChangeState(typeof(AIWalkingRightState));
      //   }
      // }
    }
  }
}