using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Wall Jump")]
    public class WallJumpAbility : MonoBehaviour, IAbility
    {
      public bool HasAbility;

      [HideInInspector]
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private CharacterControllerState2D _state;
      private Animator _animator;
      private InputActionReference _wallJumpInput;
      private InputActionReference _movement;
      private WallJumpSettings _wallJumpSettings;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
      }

      private void Start()
      {
        _animator = _character.Context.animator;
        _controller = _character.Context.controller;
        _state = _character.Context.controller.State;
        _movement = _character.Context.inputPack.Movement;
        _wallJumpInput = _character.Context.inputPack.JumpInput;
        _wallJumpSettings = _character.Context.settingsPack.WallJumpSettings;

        // Setup events
        OnEnable();
      }

      private void OnJumpStarted(InputAction.CallbackContext ctx)
      {
        if (HasAbility && _state.IsWallClinging)
        {
          WallJump();
        }
      }

      private void WallJump()
      {
        _transform.SpawnFromPool("Effects", _wallJumpSettings.JumpEffects);
        _animator.Play(_wallJumpSettings.WallJumpClip);
        _state.IsWallJumping = true;
        _controller.SlowFall(0f);

        Vector2 _movementVector = _movement.action.ReadValue<Vector2>();
        float _horizontalInput = _movementVector.x;
        bool isClingingLeft = _state.IsCollidingLeft && _horizontalInput <= -_wallJumpSettings.Threshold;
        bool isClingingRight = _state.IsCollidingRight && _horizontalInput >= _wallJumpSettings.Threshold;

        float wallJumpDirection;
        if (isClingingRight)
        {
          wallJumpDirection = -1f;
        }
        else
        {
          wallJumpDirection = 1f;
        }

        Vector2 wallJumpVector = new Vector2(wallJumpDirection * _wallJumpSettings.WallJumpForce.x, Mathf.Sqrt(2f * _wallJumpSettings.WallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity)));

        _controller.AddForce(wallJumpVector);
      }

      private void OnEnable()
      {
        // Make sure to setup new events
        OnDisable();

        if (_wallJumpInput != null)
        {
          _wallJumpInput.action.Enable();
          _wallJumpInput.action.started += OnJumpStarted;
        }
      }

      private void OnDisable()
      {
        if (_wallJumpInput != null)
        {
          _wallJumpInput.action.Disable();
          _wallJumpInput.action.started -= OnJumpStarted;
        }
      }

    }
  }
}