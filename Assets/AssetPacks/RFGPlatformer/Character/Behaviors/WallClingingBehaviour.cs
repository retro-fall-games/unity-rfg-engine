using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/Wall Clinging Behaviour")]
  public class WallClingingBehaviour : CharacterBehaviour
  {

    [Range(0.01f, 1f)]
    public float wallClingingSlowFactor = 0.6f;
    public float raycastVerticalOffset = 0f;
    public float wallClingingTolerance = 0.3f;

    public override void ProcessBehaviour()
    {
      if (_character.Controller.State.IsGrounded || _character.Controller.Velocity.y >= 0)
      {
        _character.Controller.SlowFall(0f);
        return;
      }

      bool isClingingLeft = _character.Controller.State.IsCollidingLeft && _horizontalInput <= -_character.CharacterInput.InputManager.threshold.x;
      bool isClingingRight = _character.Controller.State.IsCollidingRight && _horizontalInput >= _character.CharacterInput.InputManager.threshold.x;

      if (isClingingLeft || isClingingRight)
      {
        EnterWallClinging();
      }

      ExitWallClinging();
      WallClingingLastFrame();
    }

    private void EnterWallClinging()
    {
      // Slow the fall speed
      _character.Controller.SlowFall(wallClingingSlowFactor);
      _character.MovementState.ChangeState(MovementStates.WallClinging);
    }

    private void ExitWallClinging()
    {
      if (_character.MovementState.CurrentState == MovementStates.WallClinging)
      {
        bool shouldExit = false;
        if (_character.Controller.State.IsGrounded || _character.Controller.Velocity.y >= 0)
        {
          // If the character is grounded or moving up
          shouldExit = true;
        }

        Vector3 raycastOrigin = _transform.position;
        Vector3 raycastDirection;

        if (_character.Controller.State.IsFacingRight)
        {
          raycastOrigin = raycastOrigin + _transform.right * _character.Controller.Width() / 2 + _transform.up * raycastVerticalOffset;
          raycastDirection = _transform.right - _transform.up;
        }
        else
        {
          raycastOrigin = raycastOrigin - _transform.right * _character.Controller.Width() / 2 + _transform.up * raycastVerticalOffset;
          raycastDirection = -_transform.right - _transform.up;
        }

        LayerMask mask = _character.Controller.platformMask & (~_character.Controller.oneWayPlatformMask | ~_character.Controller.oneWayMovingPlatformMask);

        RaycastHit2D hit = RFG.Physics2D.Raycast(raycastOrigin, raycastDirection, wallClingingTolerance, mask, Color.red);

        if (_character.Controller.State.IsFacingRight)
        {
          if (!hit || _horizontalInput <= _character.CharacterInput.InputManager.threshold.x)
          {
            shouldExit = true;
          }
        }
        else
        {
          if (!hit || _horizontalInput >= -_character.CharacterInput.InputManager.threshold.x)
          {
            shouldExit = true;
          }
        }
        if (shouldExit)
        {
          ProcessExit();
        }
      }
    }

    private void ProcessExit()
    {
      _character.Controller.SlowFall(0f);
      _character.MovementState.ChangeState(MovementStates.Idle);
    }

    private void WallClingingLastFrame()
    {
      if (_character.MovementState.PreviousState == MovementStates.WallClinging && _character.MovementState.CurrentState != MovementStates.WallClinging)
      {
        _character.Controller.SlowFall(0f);
      }
    }

  }
}