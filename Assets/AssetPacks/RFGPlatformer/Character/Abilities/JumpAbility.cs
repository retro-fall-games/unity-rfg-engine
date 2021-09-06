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
      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

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


      public void SetNumberOfJumpsLeft(int numberLeft)
      {
        _numberOfJumpsLeft = numberLeft;
      }

      private bool EvaluateJumpConditions()
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

        float _verticalInput = XYAxis.action.ReadValue<Vector2>().y;

        // if the character is standing on a one way platform and is also pressing the down button,
        if (_verticalInput < -JumpSettings.JumpThreshold.y && _controller.State.IsGrounded)
        {
          if (JumpDownFromOneWayPlatform())
          {
            return false;
          }
        }

        // if the character is standing on a moving platform and not pressing the down button,
        if (_controller.State.IsGrounded)
        {
          JumpFromMovingPlatform();
        }

        return true;
      }

      private void JumpStart()
      {
        if (EvaluateJumpConditions())
        {
          _transform.SpawnFromPool("Effects", JumpSettings.JumpEffects);
          _animator.Play(JumpSettings.JumpingClip);
          _numberOfJumpsLeft--;

          _controller.GravityActive(true);
          _controller.CollisionsOn();

          _lastJumpTime = Time.time;
          _controller.State.IsIdle = false;
          _controller.State.IsWalking = false;
          _controller.State.IsFalling = false;
          _controller.State.IsJumping = true;

          _controller.SetVerticalForce(Mathf.Sqrt(2f * JumpSettings.JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
        }
      }

      private void JumpStop()
      {
        if (JumpSettings.JumpIsProportionalToThePressTime)
        {
          bool hasMinAirTime = Time.time - _lastJumpTime >= JumpSettings.JumpMinAirTime;
          bool speedGreaterThanGravity = _controller.Speed.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity));
          if (hasMinAirTime && speedGreaterThanGravity)
          {
            _lastJumpTime = 0f;
            if (JumpSettings.JumpReleaseForceFactor == 0f)
            {
              _controller.SetVerticalForce(0f);
            }
            else
            {
              _controller.AddVerticalForce(-_controller.Speed.y / JumpSettings.JumpReleaseForceFactor);
            }
          }
        }
        _controller.State.IsFalling = true;
        _controller.State.IsJumping = false;
      }

      /// <summary>
      /// Handles jumping down from a one way platform.
      /// </summary>
      protected virtual bool JumpDownFromOneWayPlatform()
      {
        if (!JumpSettings.CanJumpDownOneWayPlatforms)
        {
          return false;
        }
        if (_controller.OneWayPlatformMask.Contains(_controller.StandingOn.layer)
          || _controller.OneWayMovingPlatformMask.Contains(_controller.StandingOn.layer)
          || _controller.StairsMask.Contains(_controller.StandingOn.layer))
        {
          _controller.State.IsJumping = true;
          // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid platform
          StartCoroutine(_controller.DisableCollisionsWithOneWayPlatforms(JumpSettings.OneWayPlatformsJumpCollisionOffDuration));
          return true;
        }
        else
        {
          return false;
        }
      }

      /// <summary>
      /// Handles jumping from a moving platform.
      /// </summary>
      protected virtual void JumpFromMovingPlatform()
      {
        if (_controller.MovingPlatformMask.Contains(_controller.StandingOn.layer)
          || _controller.OneWayMovingPlatformMask.Contains(_controller.StandingOn.layer))
        {
          // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid air
          StartCoroutine(_controller.DisableCollisionsWithMovingPlatforms(JumpSettings.MovingPlatformsJumpCollisionOffDuration));
        }
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
        XYAxis.action.Enable();
        JumpInput.action.Enable();
        JumpInput.action.started += OnJumpStarted;
        JumpInput.action.canceled += OnJumpCanceled;
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
        JumpInput.action.Disable();
        JumpInput.action.started -= OnJumpStarted;
        JumpInput.action.canceled -= OnJumpCanceled;
      }

    }
  }
}