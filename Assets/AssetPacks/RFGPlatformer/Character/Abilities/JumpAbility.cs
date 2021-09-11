using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Jump")]
    public class JumpAbility : MonoBehaviour, IAbility
    {
      [HideInInspector]
      private StateCharacterContext _context;
      private Character _character;
      private Transform _transform;
      private CharacterController2D _controller;
      private Animator _animator;
      private CharacterControllerState2D _state;
      private InputActionReference _movement;
      private InputActionReference _jumpInput;
      private JumpSettings _jumpSettings;
      private int _numberOfJumpsLeft = 0;
      private float _lastJumpTime = 0f;

      private void Start()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _context = _character.Context;
        _animator = _context.animator;
        _controller = _context.controller;
        _state = _context.controller.State;
        _movement = _context.inputPack.Movement;
        _jumpInput = _context.inputPack.JumpInput;
        _jumpSettings = _context.settingsPack.JumpSettings;

        // Setup events
        OnEnable();
      }

      private void LateUpdate()
      {
        if (_state.JustGotGrounded)
        {
          _transform.SpawnFromPool("Effects", _jumpSettings.LandEffects);
          SetNumberOfJumpsLeft(_jumpSettings.NumberOfJumps);
        }
      }

      public void SetNumberOfJumpsLeft(int numberLeft)
      {
        _numberOfJumpsLeft = numberLeft;
      }

      private bool EvaluateJumpConditions()
      {
        if (_jumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }

        if (_jumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpOnGround && _numberOfJumpsLeft <= 0)
        {
          return false;
        }

        if (_state.IsWallClinging)
        {
          return false;
        }

        float _verticalInput = _movement.action.ReadValue<Vector2>().y;

        // if the character is standing on a one way platform and is also pressing the down button,
        if (_verticalInput < -_jumpSettings.JumpThreshold.y && _state.IsGrounded)
        {
          if (JumpDownFromOneWayPlatform())
          {
            return false;
          }
        }

        // if the character is standing on a moving platform and not pressing the down button,
        if (_state.IsGrounded)
        {
          JumpFromMovingPlatform();
        }

        return true;
      }

      private void JumpStart()
      {
        if (EvaluateJumpConditions())
        {
          _transform.SpawnFromPool("Effects", _jumpSettings.JumpEffects);
          _animator.Play(_jumpSettings.JumpingClip);
          _numberOfJumpsLeft--;

          _controller.GravityActive(true);
          _controller.CollisionsOn();

          _lastJumpTime = Time.time;
          _state.IsIdle = false;
          _state.IsWalking = false;
          _state.IsFalling = false;
          _state.IsJumping = true;

          _controller.SetVerticalForce(Mathf.Sqrt(2f * _jumpSettings.JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
        }
      }

      private void JumpStop()
      {
        if (_jumpSettings.JumpIsProportionalToThePressTime)
        {
          bool hasMinAirTime = Time.time - _lastJumpTime >= _jumpSettings.JumpMinAirTime;
          bool speedGreaterThanGravity = _controller.Speed.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity));
          if (hasMinAirTime && speedGreaterThanGravity)
          {
            _lastJumpTime = 0f;
            if (_jumpSettings.JumpReleaseForceFactor == 0f)
            {
              _controller.SetVerticalForce(0f);
            }
            else
            {
              _controller.AddVerticalForce(-_controller.Speed.y / _jumpSettings.JumpReleaseForceFactor);
            }
          }
        }
        _state.IsFalling = true;
        _state.IsJumping = false;
      }

      /// <summary>
      /// Handles jumping down from a one way platform.
      /// </summary>
      protected virtual bool JumpDownFromOneWayPlatform()
      {
        if (!_jumpSettings.CanJumpDownOneWayPlatforms)
        {
          return false;
        }
        if (_controller.OneWayPlatformMask.Contains(_controller.StandingOn.layer)
          || _controller.OneWayMovingPlatformMask.Contains(_controller.StandingOn.layer)
          || _controller.StairsMask.Contains(_controller.StandingOn.layer))
        {
          _state.IsJumping = true;
          // we turn the boxcollider off for a few milliseconds, so the character doesn't get stuck mid platform
          StartCoroutine(_controller.DisableCollisionsWithOneWayPlatforms(_jumpSettings.OneWayPlatformsJumpCollisionOffDuration));
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
          StartCoroutine(_controller.DisableCollisionsWithMovingPlatforms(_jumpSettings.MovingPlatformsJumpCollisionOffDuration));
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
        // Make sure to setup new events
        OnDisable();

        if (_jumpInput != null)
        {
          _jumpInput.action.Enable();
          _jumpInput.action.started += OnJumpStarted;
          _jumpInput.action.canceled += OnJumpCanceled;
        }
      }

      private void OnDisable()
      {
        if (_jumpInput != null)
        {
          _jumpInput.action.Disable();
          _jumpInput.action.started -= OnJumpStarted;
          _jumpInput.action.canceled -= OnJumpCanceled;
        }
      }

    }
  }
}