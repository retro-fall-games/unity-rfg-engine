using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/Dangling")]
    public class DanglingBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>Dangling Settings to know raycast information</summary>
      [Tooltip("Dangling Settings to know raycast information")]
      public DanglingSettings DanglingSettings;

      [HideInInspector]
      protected Vector3 _leftOne = new Vector3(-1, 1, 1);
      private CharacterController2D _controller;
      private Animator _animator;
      private Transform _transform;

      private void Awake()
      {
        _transform = transform;
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
      }

      private void Update()
      {
        // if we're dangling and not grounded, we change our state to Falling
        if (!_controller.State.IsGrounded && _controller.State.IsDangling)
        {
          _controller.State.IsFalling = true;
        }

        // if (!_controller.State.IsDangling && _startFeedbackIsPlaying)
        // {
        //   StopStartFeedbacks();
        //   PlayAbilityStopFeedbacks();
        // }

        // if dangling is disabled or if we're not grounded, we do nothing and exit
        if (_controller.State.IsWalking || _controller.State.IsRunning || _controller.State.IsJumping || _controller.State.IsDashing || !_controller.State.IsGrounded)
        {
          return;
        }

        // we determine the ray's origin (our character's position + an offset defined in the inspector)
        Vector3 raycastOrigin = Vector3.zero;
        if (_controller.State.IsFacingRight)
        {
          raycastOrigin = _transform.position + DanglingSettings.DanglingRaycastOrigin.x * Vector3.right + DanglingSettings.DanglingRaycastOrigin.y * _transform.up;
        }
        else
        {
          raycastOrigin = _transform.position - DanglingSettings.DanglingRaycastOrigin.x * Vector3.right + DanglingSettings.DanglingRaycastOrigin.y * _transform.up;
        }

        // we cast our ray downwards
        RaycastHit2D hit = RFG.Physics2D.RayCast(raycastOrigin, -_transform.up, DanglingSettings.DanglingRaycastLength, _controller.PlatformMask | _controller.OneWayPlatformMask | _controller.OneWayMovingPlatformMask, Color.gray, true);

        // if the ray didn't hit something, we're dangling
        if (!hit)
        {
          // if this is the first time we dangle, we start our feedback
          if (!_controller.State.IsDangling)
          {
            _transform.SpawnFromPool("Effects", DanglingSettings.DanglingEffects);
          }
          if (!DanglingSettings.DanglingClip.Equals(""))
          {
            _animator.Play(DanglingSettings.DanglingClip);
          }
          _controller.State.IsDangling = true;
        }

        // if the ray hit something and we were dangling previously, we go back to Idle
        if (hit && _controller.State.IsDangling)
        {
          _controller.State.IsDangling = false;
          _controller.State.IsIdle = true;
        }
      }
    }
  }
}
