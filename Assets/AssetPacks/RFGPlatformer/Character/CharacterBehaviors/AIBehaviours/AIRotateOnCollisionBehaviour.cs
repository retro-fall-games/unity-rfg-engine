using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Platformer/Character/Behaviour/AI Rotate On Collision Behaviour")]
    public class AIRotateOnCollisionBehaviour : CharacterBehaviour
    {
      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        // if (_character.AIState.CurrentState != AIStates.Wandering)
        // {
        //   return;
        // }
        // if (_character.Controller.State.IsCollidingRight)
        // {
        //   _character.AIMovementState.ChangeState(AIMovementStates.WalkingLeft);
        // }
        // else if (_character.Controller.State.IsCollidingLeft)
        // {
        //   _character.AIMovementState.ChangeState(AIMovementStates.WalkingRight);
        // }
      }
    }
  }
}