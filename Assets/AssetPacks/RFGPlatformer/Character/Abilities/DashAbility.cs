using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Dash")]
    public class DashAbility : MonoBehaviour, IAbility
    {
      public bool HasAbility;
      public Aim Aim;

      [HideInInspector]
      private StateCharacterContext _context;
      private Character _character;
      private Transform _transform;
      private CharacterController2D _controller;
      private Animator _animator;
      private CharacterControllerState2D _state;
      private InputActionReference _movement;
      private InputActionReference _dashInput;
      private DashSettings _dashSettings;
      private Vector2 _dashDirection;
      private float _slopeAngleSave = 0f;
      private float _distanceTraveled = 0f;
      private bool _shouldKeepDashing = true;
      private Vector3 _initialPosition;
      private float _cooldownTimestamp;
      private IEnumerator _dashCoroutine;
      private float _lastDashAt = 0f;
      private int _numberOfDashesLeft = 2;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
      }

      private void Start()
      {
        _context = _character.Context;
        _animator = _context.animator;
        _controller = _context.controller;
        _state = _context.controller.State;
        _movement = _context.inputPack.Movement;
        _dashInput = _context.inputPack.DashInput;
        _dashSettings = _context.settingsPack.DashSettings;

        // Setup events
        OnEnable();

        // Setup ability
        _cooldownTimestamp = 0;
        _numberOfDashesLeft = _dashSettings.TotalDashes;
        Aim.Init();
      }

      private void LateUpdate()
      {
        if (_state.IsDashing)
        {
          _controller.GravityActive(false);
        }
        HandleAmountOfDashesLeft();
      }

      private void StartDash()
      {
        if (_cooldownTimestamp > Time.time)
        {
          return;
        }

        if (_numberOfDashesLeft <= 0)
        {
          return;
        }

        _transform.SpawnFromPool("Effects", _dashSettings.DashEffects);
        _animator.Play(_dashSettings.DashingClip);

        _state.IsDashing = true;
        _cooldownTimestamp = Time.time + _dashSettings.Cooldown;
        _distanceTraveled = 0f;
        _shouldKeepDashing = true;
        _initialPosition = _transform.position;
        _lastDashAt = Time.time;

        _numberOfDashesLeft--;

        _slopeAngleSave = _controller.Parameters.MaxSlopeAngle;
        _controller.Parameters.MaxSlopeAngle = 0;

        ComputerDashDirection();
        CheckFlipCharacter();

        _dashCoroutine = Dash();
        StartCoroutine(_dashCoroutine);
      }

      private void ComputerDashDirection()
      {
        Aim.PrimaryMovement = _movement.action.ReadValue<Vector2>();
        Aim.CurrentPosition = _transform.position;
        _dashDirection = Aim.GetCurrentAim();

        CheckAutoCorrectTrajectory();

        if (_dashDirection.magnitude < _dashSettings.MinInputThreshold)
        {
          _dashDirection = _state.IsFacingRight ? Vector2.right : Vector2.left;
        }
        else
        {
          _dashDirection = _dashDirection.normalized;
        }
      }

      private void CheckAutoCorrectTrajectory()
      {
        if (_state.IsCollidingBelow && _dashDirection.y < 0f)
        {
          _dashDirection.y = 0f;
        }
      }

      private void CheckFlipCharacter()
      {
        if (Mathf.Abs(_dashDirection.x) > 0.05f)
        {
          if (_state.IsFacingRight != _dashDirection.x > 0f)
          {
            _controller.Flip();
          }
        }
      }

      private IEnumerator Dash()
      {
        while (_distanceTraveled < _dashSettings.DashDistance && _shouldKeepDashing && _state.IsDashing)
        {
          _distanceTraveled = Vector3.Distance(_initialPosition, _transform.position);

          if ((_state.IsCollidingLeft && _dashDirection.x < 0f)
            || (_state.IsCollidingRight && _dashDirection.x > 0f)
            || (_state.IsCollidingAbove && _dashDirection.y > 0f)
            || (_state.IsCollidingBelow && _dashDirection.y < 0f))
          {
            _shouldKeepDashing = false;
            _controller.SetForce(Vector2.zero);
          }
          else
          {
            _controller.GravityActive(false);
            _controller.SetForce(_dashDirection * _dashSettings.DashForce);
          }
          yield return null;
        }
        StopDash();
      }

      private void StopDash()
      {
        if (_dashCoroutine != null)
        {
          StopCoroutine(_dashCoroutine);
        }

        _controller.DefaultParameters.MaxSlopeAngle = _slopeAngleSave;
        _controller.Parameters.MaxSlopeAngle = _slopeAngleSave;
        _controller.GravityActive(true);
        _controller.SetForce(Vector2.zero);

        if (_state.IsDashing)
        {
          _state.IsDashing = false;
        }
      }

      public void SetNumberOfDashesLeft(int numberLeft)
      {
        _numberOfDashesLeft = numberLeft;
      }

      private void HandleAmountOfDashesLeft()
      {
        if (Time.time - _lastDashAt < _dashSettings.Cooldown)
        {
          return;
        }

        if (_state.IsGrounded)
        {
          SetNumberOfDashesLeft(_dashSettings.TotalDashes);
        }
      }

      public void OnDashStarted(InputAction.CallbackContext ctx)
      {
        if (!HasAbility)
          return;
        StartDash();
      }

      private void OnEnable()
      {
        // Make sure to setup new events
        OnDisable();

        if (_dashInput != null)
        {
          _dashInput.action.Enable();
          _dashInput.action.started += OnDashStarted;
        }
      }

      private void OnDisable()
      {
        if (_dashInput != null)
        {
          _dashInput.action.Disable();
          _dashInput.action.started -= OnDashStarted;
        }
      }

    }
  }
}