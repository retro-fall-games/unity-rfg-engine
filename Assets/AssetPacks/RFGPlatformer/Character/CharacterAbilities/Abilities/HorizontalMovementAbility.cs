using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Horizontal Movement Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Horizontal Movement")]
    public class HorizontalMovementAbility : CharacterAbility
    {
      [Header("Settings")]
      public float Speed = 5f;

      [Header("Sound FX")]
      public SoundData[] MovementFx;

      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;
      private float _horizontalSpeed;

      public override void Init(Character character)
      {
        _character = character;
        _controller = character.Controller;
        _movement = character.Input.Movement;
      }

      public override void EarlyProcess()
      {
        _movementVector = _movement.ReadValue<Vector2>();
      }

      public override void Process()
      {

        _horizontalSpeed = _movementVector.x;
        // float _verticalInput = inputMovement.y;

        if (_horizontalSpeed > 0f)
        {
          if (!_controller.State.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }
        else if (_horizontalSpeed < 0f)
        {
          if (_controller.State.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }

        // If the movement state is dashing return so it wont get set back to idle
        if (_character.MovementState == MovementState.Dashing)
        {
          return;
        }

        // Call the Use method to call any SoundFx
        if (_horizontalSpeed != 0f)
        {
          if (MovementFx.Length > 0)
          {
            SoundManager.Instance.Play(MovementFx);
          }
        }

        // if (_verticalInput >= 1 || _verticalInput <= -1)
        // {
        //   _controller.CollisionsOnStairs(true);
        // }

        if ((!_controller.State.IsJumping && !_controller.State.IsFalling && _controller.State.IsGrounded) || _controller.State.JustGotGrounded)
        {
          _character.CharacterMovementState.ChangeState(_horizontalSpeed == 0 ? typeof(IdleState) : typeof(WalkingState));
        }

        float movementFactor = _controller.State.IsGrounded ? _controller.Parameters.GroundSpeedFactor : _controller.Parameters.AirSpeedFactor;
        float movementSpeed = _horizontalSpeed * Speed * _controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        _controller.SetHorizontalForce(horizontalMovementForce);
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }

    }
  }
}