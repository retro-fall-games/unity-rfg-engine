using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Platformer/Character/Behaviour/AI Movement Path Behaviour")]
    public class AIMovementPathBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float walkSpeed = 5f;

      public MovementPath movementPath;

#if UNITY_EDITOR
      [ButtonMethod]
      private void AddMovementPath()
      {
        // this.movementPath = gameObject.AddComponent<MovementPath>();
      }
#endif

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        // if (_character.AIState.CurrentState != AIStates.MovementPath || movementPath == null)
        // {
        //   _character.MovementState.ChangeState(MovementStates.Idle);
        //   return;
        // }

        // _character.Controller.RotateTowards(movementPath.NextPath);
        // if (!movementPath.autoMove)
        // {
        //   movementPath.Move();
        //   movementPath.CheckPath();
        // }

        // if (movementPath.state == MovementPath.State.OneWay && movementPath.ReachedEnd)
        // {
        //   _character.MovementState.ChangeState(MovementStates.Idle);
        // }
        // else
        // {
        //   _character.MovementState.ChangeState(MovementStates.Walking);
        // }

      }

    }
  }
}