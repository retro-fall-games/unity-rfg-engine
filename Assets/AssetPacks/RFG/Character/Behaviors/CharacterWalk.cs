using UnityEngine;

namespace RFG.Character
{
  public class CharacterWalk : CharacterBehavior
  {
    public float walkSpeed = 5f;
    public float inputThreshold = 0.1f;
    private float _horizontalMovement;
    private float _normalizedHorizontalSpeed;
    private float _horizontalMovementForce;
    private float _lastGroundedHorizontalMovement;

    public override void Process()
    {
      HandleInput();
      HandleMovement();
    }

    private void HandleInput()
    {
      _horizontalMovement = _character.InputManager.PrimaryMovement.x;
    }

    private void HandleMovement()
    {
      if (_horizontalMovement > inputThreshold)
      {
        _normalizedHorizontalSpeed = _horizontalMovement;
      }
      else if (_horizontalMovement < inputThreshold)
      {
        _normalizedHorizontalSpeed = _horizontalMovement;
      }
      else
      {
        _normalizedHorizontalSpeed = 0;
      }

      // If we're grounded and moving, currently idle, dangling, or falling, we become walking
      if (_normalizedHorizontalSpeed != 0 && _movementState.CurrentState == MovementStates.Idle)
      {
        _movementState.ChangeState(MovementStates.Walking);
      }

      // If we're walking and not moving anymore, we go back to idle
      if (_normalizedHorizontalSpeed == 0 && _movementState.CurrentState == MovementStates.Walking)
      {
        _movementState.ChangeState(MovementStates.Idle);
      }

      float movementFactor = 1f;
      float movementSpeed = _normalizedHorizontalSpeed * walkSpeed;

      _horizontalMovementForce = Mathf.Lerp(_controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

      _controller.SetHorizontalForce(_horizontalMovementForce);

      if (_controller.CollisionState.IsGrounded)
      {
        _lastGroundedHorizontalMovement = _horizontalMovement;
      }
    }
  }
}