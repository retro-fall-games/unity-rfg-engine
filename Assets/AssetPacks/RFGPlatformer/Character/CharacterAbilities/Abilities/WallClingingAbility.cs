using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Clinging Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Wall Clinging")]
    public class WallClingingAbility : CharacterAbility
    {
      [Header("Settings")]
      [Range(0.01f, 1f)]
      public float WallClingingSlowFactor = 0.6f;
      public float RaycastVerticalOffset = 0f;
      public float WallClingingTolerance = 0.3f;
      public float Threshold = 0.1f;

      [Header("Sound FX")]
      public SoundData[] ClingFx;

      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;

      public override void Init(Character character)
      {
        _character = character;
        _transform = character.transform;
        _controller = character.Controller;
        _movement = character.Input.Movement;
      }

      public override void EarlyProcess()
      {
        _movementVector = _movement.ReadValue<Vector2>();
      }

      public override void Process()
      {
        if (_controller.State.IsGrounded || _controller.Velocity.y >= 0)
        {
          _controller.SlowFall(0f);
          return;
        }

        float _horizontalInput = _movementVector.x;
        float _verticalInput = _movementVector.y;

        bool isClingingLeft = _controller.State.IsCollidingLeft && _horizontalInput <= -Threshold;
        bool isClingingRight = _controller.State.IsCollidingRight && _horizontalInput >= Threshold;

        // If we are wall clinging, then change the state
        if (isClingingLeft || isClingingRight)
        {
          // Slow the fall speed
          _controller.SlowFall(WallClingingSlowFactor);
          _character.CharacterMovementState.ChangeState(typeof(WallClingingState));
        }

        // If we are in a wall clinging state then make sure we are still wall clinging
        // if not then go back to idle
        if (_character.CharacterMovementState.CurrentStateType == typeof(WallClingingState))
        {
          bool shouldExit = false;
          if (_controller.State.IsGrounded || _controller.Velocity.y >= 0)
          {
            // If the character is grounded or moving up
            shouldExit = true;
          }

          Vector3 raycastOrigin = _transform.position;
          Vector3 raycastDirection;
          Vector3 right = _transform.right;

          if (isClingingRight && !_controller.State.IsFacingRight)
          {
            right = -right;
          }
          else if (isClingingLeft && _controller.State.IsFacingRight)
          {
            right = -right;
          }

          raycastOrigin = raycastOrigin + right * _controller.Width() / 2 + _transform.up * RaycastVerticalOffset;
          raycastDirection = right - _transform.up;

          LayerMask mask = _controller.platformMask & (~_controller.oneWayPlatformMask | ~_controller.oneWayMovingPlatformMask);

          RaycastHit2D hit = RFG.Physics2D.Raycast(raycastOrigin, raycastDirection, WallClingingTolerance, mask, Color.red);

          if (isClingingRight)
          {
            if (!hit || _horizontalInput <= Threshold)
            {
              shouldExit = true;
            }
          }
          else
          {
            if (!hit || _horizontalInput >= -Threshold)
            {
              shouldExit = true;
            }
          }
          if (shouldExit)
          {
            _controller.SlowFall(0f);
            _character.CharacterMovementState.ChangeState(typeof(FallingState));
          }
        }

        if (_character.CharacterMovementState.PreviousStateType == typeof(WallClingingState) && _character.CharacterMovementState.CurrentStateType != typeof(WallClingingState))
        {
          _controller.SlowFall(0f);
        }
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