using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/Wall Clinging Behaviour")]
  public class WallClingingBehaviour : PlatformerCharacterBehaviour
  {

    [Range(0.01f, 1f)]
    public float wallClingingSlowFactor = 0.6f;
    public float raycastVerticalOffset = 0f;
    public float wallClingingTolerance = 0.3f;

    private CharacterController2D _controller;
    private CharacterControllerState2D _state;
    private InputManager _input;

    public override void InitBehaviour()
    {
      _controller = _character.Controller;
      _state = _character.Controller.State;
      _input = InputManager.Instance;
    }

    public override void ProcessBehaviour()
    {
      if (_state.IsGrounded || _controller.Velocity.y >= 0 || !authorized)
      {
        _controller.SlowFall(0f);
        return;
      }

      float _horizontalInput = _input.PrimaryMovement.x;
      float _verticalInput = _input.PrimaryMovement.y;

      float threshold = _input.threshold.x;
      bool isClingingLeft = _state.IsCollidingLeft && _horizontalInput <= -threshold;
      bool isClingingRight = _state.IsCollidingRight && _horizontalInput >= threshold;

      // If we are wall clinging, then change the state
      if (isClingingLeft || isClingingRight)
      {
        // Slow the fall speed
        _controller.SlowFall(wallClingingSlowFactor);
        _character.MovementState.ChangeState(MovementStates.WallClinging);
      }

      // If we are in a wall clinging state then make sure we are still wall clinging
      // if not then go back to idle
      if (_character.MovementState.CurrentState == MovementStates.WallClinging)
      {
        bool shouldExit = false;
        if (_state.IsGrounded || _controller.Velocity.y >= 0)
        {
          // If the character is grounded or moving up
          shouldExit = true;
        }

        Vector3 raycastOrigin = _transform.position;
        Vector3 raycastDirection;
        Vector3 right = _transform.right;

        if (isClingingRight && !_state.IsFacingRight)
        {
          right = -right;
        }
        else if (isClingingLeft && _state.IsFacingRight)
        {
          right = -right;
        }

        raycastOrigin = raycastOrigin + right * _controller.Width() / 2 + _transform.up * raycastVerticalOffset;
        raycastDirection = right - _transform.up;

        LayerMask mask = _controller.platformMask & (~_controller.oneWayPlatformMask | ~_controller.oneWayMovingPlatformMask);

        RaycastHit2D hit = RFG.Physics2D.Raycast(raycastOrigin, raycastDirection, wallClingingTolerance, mask, Color.red);

        if (isClingingRight)
        {
          if (!hit || _horizontalInput <= threshold)
          {
            shouldExit = true;
          }
        }
        else
        {
          if (!hit || _horizontalInput >= -threshold)
          {
            shouldExit = true;
          }
        }
        if (shouldExit)
        {
          _controller.SlowFall(0f);
          _character.MovementState.ChangeState(MovementStates.Idle);
        }
      }

      if (_character.MovementState.PreviousState == MovementStates.WallClinging && _character.MovementState.CurrentState != MovementStates.WallClinging)
      {
        _controller.SlowFall(0f);
      }
    }

  }
}