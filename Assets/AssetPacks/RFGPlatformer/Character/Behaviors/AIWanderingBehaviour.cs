using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/AI Wandering Behaviour")]
  public class AIWanderingBehaviour : PlatformerCharacterBehaviour
  {
    [Header("Settings")]
    public float walkSpeed = 5f;

    public override void ProcessBehaviour()
    {
      if (_character.AIState.CurrentState != AIStates.Wandering)
      {
        return;
      }

      float _normalizedHorizontalSpeed = 0f;

      if (_character.AIMovementState.CurrentState == AIMovementStates.WalkingRight)
      {
        _normalizedHorizontalSpeed = 1f;
        if (!_character.Controller.State.IsFacingRight)
        {
          _character.Controller.Flip();
        }
      }
      else if (_character.AIMovementState.CurrentState == AIMovementStates.WalkingLeft)
      {
        _normalizedHorizontalSpeed = -1f;
        if (_character.Controller.State.IsFacingRight)
        {
          _character.Controller.Flip();
        }
      }
      else if (_character.AIMovementState.CurrentState == AIMovementStates.Idle)
      {
        _normalizedHorizontalSpeed = 0f;
      }

      _character.MovementState.ChangeState(_normalizedHorizontalSpeed == 0 ? MovementStates.Idle : MovementStates.Walking);

      float movementFactor = _character.Controller.State.IsGrounded ? _character.Controller.Parameters.groundSpeedFactor : _character.Controller.Parameters.airSpeedFactor;
      float movementSpeed = _normalizedHorizontalSpeed * walkSpeed * _character.Controller.Parameters.speedFactor;
      float horizontalMovementForce = Mathf.Lerp(_character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

      _character.Controller.SetHorizontalForce(horizontalMovementForce);

    }

  }
}