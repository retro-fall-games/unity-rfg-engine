using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Jump")]
    public class JumpAbility : MonoBehaviour, IAbility
    {
      [Header("Input")]
      /// <summary>Input Action to initiate the Jump State</summary>
      [Tooltip("Input Action to initiate the Jump State")]
      public InputActionReference JumpInput;

      [Header("Settings")]
      /// <summary>Jump Settings to know how many jumps left and jump restrictions</summary>
      [Tooltip("Jump Settings to know how many jumps left and jump restrictions")]
      public JumpSettings JumpSettings;

      [HideInInspector]
      private Transform _transform;
      private CharacterController2D _controller;
      private Animator _animator;
      private int _numberOfJumpsLeft = 0;
      private float _lastJumpTime = 0f;

      private void Awake()
      {
        _transform = transform;
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
      }

      private void LateUpdate()
      {
        if (_controller.State.JustGotGrounded)
        {
          _transform.SpawnFromPool("Effects", JumpSettings.LandEffects);
          SetNumberOfJumpsLeft(JumpSettings.NumberOfJumps);
        }
      }

      private bool CanJump()
      {
        if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }

        if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpOnGround && _numberOfJumpsLeft <= 0)
        {
          return false;
        }

        if (_controller.State.IsWallClinging)
        {
          return false;
        }

        return true;
      }

      public void SetNumberOfJumpsLeft(int numberLeft)
      {
        _numberOfJumpsLeft = numberLeft;
      }

      private void JumpStart()
      {
        if (CanJump())
        {
          _transform.SpawnFromPool("Effects", JumpSettings.JumpEffects);
          _animator.Play(JumpSettings.JumpingClip);
          _numberOfJumpsLeft--;
          // When the jump state enters then start adding velocity to the controller
          // _controller.CollisionsOnStairs(true);

          // Vector2 movementVector = ctx.input.PrimaryMovement;
          // float _verticalInput = movementVector.y;

          // if (_verticalInput < 0f)
          // {
          //   // _lastJumpTime = Time.time;
          //   // _controller.State.IsFalling = true;
          //   // _controller.State.IsJumping = false;
          //   // ctx.character.ChangeState(typeof(FallingState));
          //   // _controller.IgnoreOneWayPlatformsThisFrame = true;
          //   // _controller.SetVerticalForce(JumpSettings.OneWayPlatformFallVelocity);
          //   // _controller.IgnoreStairsForTime(0.1f);
          // }
          // else
          // {
          _lastJumpTime = Time.time;
          _controller.State.IsIdle = false;
          _controller.State.IsWalking = false;
          _controller.State.IsFalling = false;
          _controller.State.IsJumping = true;
          _controller.AddVerticalForce(Mathf.Sqrt(2f * JumpSettings.JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
          //}
        }
      }

      private void JumpStop()
      {
        if (JumpSettings.JumpIsProportionalToThePressTime)
        {
          bool hasMinAirTime = Time.time - _lastJumpTime >= JumpSettings.JumpMinAirTime;
          bool speedGreaterThanGravity = _controller.Velocity.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity));
          if (hasMinAirTime && speedGreaterThanGravity)
          {
            _lastJumpTime = 0f;
            if (JumpSettings.JumpReleaseForceFactor == 0f)
            {
              _controller.SetVerticalForce(0f);
            }
            else
            {
              _controller.AddVerticalForce(-_controller.Velocity.y / JumpSettings.JumpReleaseForceFactor);
            }
          }
        }
        _controller.State.IsFalling = true;
        _controller.State.IsJumping = false;
      }

      private void OnJumpStarted(InputAction.CallbackContext ctx)
      {
        JumpStart();
      }

      private void OnJumpCanceled(InputAction.CallbackContext ctx)
      {
        JumpStop();
      }

      private void OnEnable()
      {
        JumpInput.action.Enable();
        JumpInput.action.started += OnJumpStarted;
        JumpInput.action.canceled += OnJumpCanceled;
      }

      private void OnDisable()
      {
        JumpInput.action.Disable();
        JumpInput.action.started -= OnJumpStarted;
        JumpInput.action.canceled -= OnJumpCanceled;
      }

    }
  }
}