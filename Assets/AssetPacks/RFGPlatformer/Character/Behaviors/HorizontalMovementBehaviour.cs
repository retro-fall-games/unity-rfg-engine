using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Character/Behaviour/Horizontal Movement Behaviour")]
    public class HorizontalMovementBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float walkSpeed = 5f;

      public override void ProcessBehaviour()
      {
        // float _normalizedHorizontalSpeed = 0f;
        // float _horizontalInput = InputManager.Instance.PrimaryMovement.x;
        // float _verticalInput = InputManager.Instance.PrimaryMovement.y;

        // if (_horizontalInput > 0f)
        // {
        //   _normalizedHorizontalSpeed = 1f;
        //   if (!_character.Controller.State.IsFacingRight && !_character.Controller.rotateOnMouseCursor)
        //   {
        //     _character.Controller.Flip();
        //   }
        // }
        // else if (_horizontalInput < 0f)
        // {
        //   _normalizedHorizontalSpeed = -1f;
        //   if (_character.Controller.State.IsFacingRight && !_character.Controller.rotateOnMouseCursor)
        //   {
        //     _character.Controller.Flip();
        //   }
        // }
        // else
        // {
        //   _normalizedHorizontalSpeed = 0f;
        // }

        // If we are dashing then return here so it wont get set back to idle
        // if (_character.MovementState.CurrentState == MovementStates.Dashing)
        // {
        //   return;
        // }

        // if (_verticalInput >= 1 || _verticalInput <= -1)
        // {
        //   _character.Controller.CollisionsOnStairs(true);
        // }

        // _character.MovementState.ChangeState(_normalizedHorizontalSpeed == 0 ? MovementStates.Idle : MovementStates.Walking);

        // float movementFactor = _character.Controller.State.IsGrounded ? _character.Controller.Parameters.GroundSpeedFactor : _character.Controller.Parameters.AirSpeedFactor;
        // float movementSpeed = _normalizedHorizontalSpeed * walkSpeed * _character.Controller.Parameters.SpeedFactor;
        // float horizontalMovementForce = Mathf.Lerp(_character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        // _character.Controller.SetHorizontalForce(horizontalMovementForce);

      }

    }
  }
}