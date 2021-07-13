using UnityEngine;

namespace RFG.Platformer
{
  public class HorizontalMovementBehavior : CharacterBehavior
  {
    public float walkSpeed = 5f;

    public override void ProcessBehavior()
    {
      float _normalizedHorizontalSpeed = 0f;

      if (_horizontalInput > 0f)
      {
        _normalizedHorizontalSpeed = 1f;
        if (!_character.Controller.State.IsFacingRight)
        {
          _character.Controller.Flip();
        }
      }
      else if (_horizontalInput < 0f)
      {
        _normalizedHorizontalSpeed = -1f;
        if (_character.Controller.State.IsFacingRight)
        {
          _character.Controller.Flip();
        }
      }
      else
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