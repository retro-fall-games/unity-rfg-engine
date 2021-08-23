using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Wall Clinging")]
    public class WallClingingAbility : MonoBehaviour
    {
      [Header("Input")]
      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      [Header("Settings")]
      /// <summary>Wall Clinging Settings to know input thresholds</summary>
      [Tooltip("Wall Clinging Settings to know input thresholds")]
      public WallClingingSettings WallClingingSettings;

      [HideInInspector]
      private Transform _transform;
      private CharacterController2D _controller;
      private Animator _animator;

      private void Awake()
      {
        _transform = GetComponent<Transform>();
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
      }

      private void Update()
      {
        if (_controller.State.IsGrounded || _controller.Velocity.y >= 0)
        {
          _controller.SlowFall(0f);
          return;
        }

        Vector2 _movementVector = XYAxis.action.ReadValue<Vector2>();

        float _horizontalInput = _movementVector.x;
        float _verticalInput = _movementVector.y;

        bool isClingingLeft = _controller.State.IsCollidingLeft && _horizontalInput <= -WallClingingSettings.Threshold;
        bool isClingingRight = _controller.State.IsCollidingRight && _horizontalInput >= WallClingingSettings.Threshold;

        // If we are wall clinging, then change the state
        if (isClingingLeft || isClingingRight)
        {
          // Slow the fall speed
          _controller.SlowFall(WallClingingSettings.WallClingingSlowFactor);
          _controller.State.IsWallClinging = true;
        }
        else
        {
          _controller.State.IsWallClinging = false;
        }

        // If we are in a wall clinging state then make sure we are still wall clinging
        // if not then go back to idle
        if (_controller.State.IsWallClinging)
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

          raycastOrigin = raycastOrigin + right * _controller.Width() / 2 + _transform.up * WallClingingSettings.RaycastVerticalOffset;
          raycastDirection = right - _transform.up;

          LayerMask mask = _controller.platformMask & (~_controller.oneWayPlatformMask | ~_controller.oneWayMovingPlatformMask);

          RaycastHit2D hit = RFG.Physics2D.Raycast(raycastOrigin, raycastDirection, WallClingingSettings.WallClingingTolerance, mask, Color.red);

          if (isClingingRight)
          {
            if (!hit || _horizontalInput <= WallClingingSettings.Threshold)
            {
              shouldExit = true;
            }
          }
          else
          {
            if (!hit || _horizontalInput >= -WallClingingSettings.Threshold)
            {
              shouldExit = true;
            }
          }
          if (shouldExit)
          {
            _controller.SlowFall(0f);
            _controller.State.IsFalling = true;
            _controller.State.IsWallClinging = false;
          }
          else
          {
            _transform.SpawnFromPool("Effects", WallClingingSettings.WallClingingEffects);
            _animator.Play(WallClingingSettings.WallClingingClip);
          }
        }

        if (!_controller.State.IsWallClinging)
        {
          _controller.SlowFall(0f);
        }

      }

      private void OnEnable()
      {
        XYAxis.action.Enable();
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
      }

    }
  }
}