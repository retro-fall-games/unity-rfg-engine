using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Character/Behaviour/AI Aggro Behaviour")]
    public class AIAggroBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float runSpeed = 5f;
      private Aggro _aggro;
      public override void InitBehaviour()
      {
        _aggro = GetComponent<Aggro>();
        _aggro.OnAggroChange += OnAggroChange;
      }

      public override void ProcessBehaviour()
      {
        // if (_character.AIState.CurrentState == AIStates.Attacking)
        // {
        //   // Rotate to always face the target
        //   _character.Controller.RotateTowards(_aggro.target2);

        //   // Move towards that target
        //   float _normalizedHorizontalSpeed = 0f;

        //   if (_character.Controller.State.IsFacingRight)
        //   {
        //     _character.AIMovementState.ChangeState(AIMovementStates.RunningRight);
        //     _normalizedHorizontalSpeed = 1f;
        //   }
        //   else
        //   {
        //     _character.AIMovementState.ChangeState(AIMovementStates.RunningLeft);
        //     _normalizedHorizontalSpeed = -1f;
        //   }

        //   _character.MovementState.ChangeState(MovementStates.Running);

        //   float movementFactor = _character.Controller.State.IsGrounded ? _character.Controller.Parameters.GroundSpeedFactor : _character.Controller.Parameters.AirSpeedFactor;
        //   float movementSpeed = _normalizedHorizontalSpeed * runSpeed * _character.Controller.Parameters.SpeedFactor;
        //   float horizontalMovementForce = Mathf.Lerp(_character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        //   _character.Controller.SetHorizontalForce(horizontalMovementForce);
        // }
      }

      private void OnAggroChange(bool aggro)
      {
        // if (aggro)
        // {
        //   _character.AIState.ChangeState(AIStates.Attacking);
        // }
        // else
        // {
        //   _character.AIState.ChangeState(AIStates.Wandering);
        // }
      }
    }
  }
}