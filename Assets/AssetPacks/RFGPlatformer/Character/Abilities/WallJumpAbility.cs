using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Wall Jump")]
    public class WallJumpAbility : MonoBehaviour, IAbility
    {
      [Header("Input")]
      /// <summary>Input Action to initiate the Wall Jump State</summary>
      [Tooltip("Input Action to initiate the Wall Jump State")]
      public InputActionReference WallJumpInput;

      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      [Header("Settings")]
      /// <summary>Wall Jump Settings to know threshold and force</summary>
      [Tooltip("Wall Jump Settings to know threshold and force")]
      public WallJumpSettings WallJumpSettings;
      public bool HasAbility;

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

      private void OnJumpStarted(InputAction.CallbackContext ctx)
      {
        if (HasAbility && _controller.State.IsWallClinging)
        {
          WallJump();
        }
      }

      private void WallJump()
      {
        _transform.SpawnFromPool("Effects", WallJumpSettings.JumpEffects);
        _animator.Play(WallJumpSettings.WallJumpClip);
        _controller.State.IsWallJumping = true;
        _controller.SlowFall(0f);

        Vector2 _movementVector = XYAxis.action.ReadValue<Vector2>();
        float _horizontalInput = _movementVector.x;
        bool isClingingLeft = _controller.State.IsCollidingLeft && _horizontalInput <= -WallJumpSettings.Threshold;
        bool isClingingRight = _controller.State.IsCollidingRight && _horizontalInput >= WallJumpSettings.Threshold;

        float wallJumpDirection;
        if (isClingingRight)
        {
          wallJumpDirection = -1f;
        }
        else
        {
          wallJumpDirection = 1f;
        }

        Vector2 wallJumpVector = new Vector2(wallJumpDirection * WallJumpSettings.WallJumpForce.x, Mathf.Sqrt(2f * WallJumpSettings.WallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity)));

        _controller.AddForce(wallJumpVector);
      }

      private void OnEnable()
      {
        XYAxis.action.Enable();
        WallJumpInput.action.Enable();
        WallJumpInput.action.started += OnJumpStarted;
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
        WallJumpInput.action.Disable();
        WallJumpInput.action.started -= OnJumpStarted;
      }

    }
  }
}