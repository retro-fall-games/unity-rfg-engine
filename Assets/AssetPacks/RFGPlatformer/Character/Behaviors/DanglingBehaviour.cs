using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/Dangling")]
    public class DanglingBehaviour : MonoBehaviour
    {
      [HideInInspector]
      protected Vector3 _leftOne = new Vector3(-1, 1, 1);
      private Character _character;
      private CharacterControllerState2D _state;
      private CharacterController2D _controller;
      private Animator _animator;
      private Transform _transform;
      private DanglingSettings _danglingSettings;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
      }

      private void Start()
      {
        _controller = _character.Context.controller;
        _animator = _character.Context.animator;
        _controller = _character.Context.controller;
        _state = _character.Context.controller.State;
        _danglingSettings = _character.Context.settingsPack.DanglingSettings;
      }

      private void Update()
      {
        // if we're dangling and not grounded, we change our state to Falling
        if (!_state.IsGrounded && _state.IsDangling)
        {
          _state.IsFalling = true;
        }

        // if dangling is disabled or if we're not grounded, we do nothing and exit
        if (_state.IsWalking || _state.IsRunning || _state.IsJumping || _state.IsDashing || !_state.IsGrounded)
        {
          return;
        }

        // we determine the ray's origin (our character's position + an offset defined in the inspector)
        Vector3 raycastOrigin = Vector3.zero;
        if (_state.IsFacingRight)
        {
          raycastOrigin = _transform.position + _danglingSettings.DanglingRaycastOrigin.x * Vector3.right + _danglingSettings.DanglingRaycastOrigin.y * _transform.up;
        }
        else
        {
          raycastOrigin = _transform.position - _danglingSettings.DanglingRaycastOrigin.x * Vector3.right + _danglingSettings.DanglingRaycastOrigin.y * _transform.up;
        }

        // we cast our ray downwards
        RaycastHit2D hit = RFG.Physics2D.RayCast(raycastOrigin, -_transform.up, _danglingSettings.DanglingRaycastLength, _controller.PlatformMask | _controller.OneWayPlatformMask | _controller.OneWayMovingPlatformMask, Color.gray, true);

        // if the ray didn't hit something, we're dangling
        if (!hit)
        {
          // if this is the first time we dangle, we start our feedback
          if (!_state.IsDangling)
          {
            _transform.SpawnFromPool("Effects", _danglingSettings.DanglingEffects);
          }
          if (!_danglingSettings.DanglingClip.Equals(""))
          {
            _animator.Play(_danglingSettings.DanglingClip);
          }
          _state.IsDangling = true;
        }

        // if the ray hit something and we were dangling previously, we go back to Idle
        if (hit && _state.IsDangling)
        {
          _state.IsDangling = false;
          _state.IsIdle = true;
        }
      }
    }
  }
}
