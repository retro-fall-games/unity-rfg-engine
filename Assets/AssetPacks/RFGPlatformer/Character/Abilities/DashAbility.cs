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
      [Header("Input")]
      /// <summary>Input Action to initiate the Jump State</summary>
      [Tooltip("Input Action to initiate the Dash State")]
      public InputActionReference DashInput;

      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      [Header("Settings")]
      /// <summary>Dash Settings to know dash distance and direction</summary>
      [Tooltip("Dash Settings to know dash distance and direction")]
      public DashSettings DashSettings;
      public Aim Aim;
      public bool HasAbility;

      [HideInInspector]
      private Vector2 _dashDirection;
      private float _distanceTraveled = 0f;
      private bool _shouldKeepDashing = true;
      private Vector3 _initialPosition;
      private float _cooldownTimestamp;
      private IEnumerator _dashCoroutine;
      private float _lastDashAt = 0f;
      public int _numberOfDashesLeft = 2;
      private Transform _transform;
      private CharacterController2D _controller;

      private void Awake()
      {
        _transform = GetComponent<Transform>();
        _controller = GetComponent<CharacterController2D>();
        _cooldownTimestamp = 0;
        _numberOfDashesLeft = DashSettings.TotalDashes;
        Aim.Init();
      }

      private void LateUpdate()
      {
        if (_controller.State.IsDashing)
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

        _transform.SpawnFromPool("Effects", DashSettings.DashEffects);

        _controller.CollisionsOnStairs(true);
        _controller.State.IsDashing = true;
        _cooldownTimestamp = Time.time + DashSettings.Cooldown;
        _distanceTraveled = 0f;
        _shouldKeepDashing = true;
        _initialPosition = _transform.position;
        _lastDashAt = Time.time;

        _numberOfDashesLeft--;

        ComputerDashDirection();
        CheckFlipCharacter();

        _dashCoroutine = Dash();
        StartCoroutine(_dashCoroutine);
      }

      private void ComputerDashDirection()
      {
        Aim.PrimaryMovement = XYAxis.action.ReadValue<Vector2>();
        Aim.CurrentPosition = _transform.position;
        _dashDirection = Aim.GetCurrentAim();

        CheckAutoCorrectTrajectory();

        if (_dashDirection.magnitude < DashSettings.MinInputThreshold)
        {
          _dashDirection = _controller.State.IsFacingRight ? Vector2.right : Vector2.left;
        }
        else
        {
          _dashDirection = _dashDirection.normalized;
        }
      }

      private void CheckAutoCorrectTrajectory()
      {
        if (_controller.State.IsCollidingBelow && _dashDirection.y < 0f)
        {
          _dashDirection.y = 0f;
        }
      }

      private void CheckFlipCharacter()
      {
        if (Mathf.Abs(_dashDirection.x) > 0.05f)
        {
          if (_controller.State.IsFacingRight != _dashDirection.x > 0f)
          {
            _controller.Flip();
          }
        }
      }

      private IEnumerator Dash()
      {
        while (_distanceTraveled < DashSettings.DashDistance && _shouldKeepDashing && _controller.State.IsDashing)
        {
          _distanceTraveled = Vector3.Distance(_initialPosition, _transform.position);

          if ((_controller.State.IsCollidingLeft && _dashDirection.x < 0f)
            || (_controller.State.IsCollidingRight && _dashDirection.x > 0f)
            || (_controller.State.IsCollidingAbove && _dashDirection.y > 0f)
            || (_controller.State.IsCollidingBelow && _dashDirection.y < 0f))
          {
            _shouldKeepDashing = false;
            _controller.SetForce(Vector2.zero);
          }
          else
          {
            _controller.GravityActive(false);
            _controller.SetForce(_dashDirection * DashSettings.DashForce);
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
        _controller.GravityActive(true);
        _controller.SetForce(Vector2.zero);

        if (_controller.State.IsDashing)
        {
          _controller.State.IsDashing = false;
        }
      }

      public void SetNumberOfDashesLeft(int numberLeft)
      {
        _numberOfDashesLeft = numberLeft;
      }

      private void HandleAmountOfDashesLeft()
      {
        if (Time.time - _lastDashAt < DashSettings.Cooldown)
        {
          return;
        }

        if (_controller.State.IsGrounded)
        {
          SetNumberOfDashesLeft(DashSettings.TotalDashes);
        }
      }

      public void OnDashStarted(InputAction.CallbackContext ctx)
      {
        if (HasAbility)
        {
          StartDash();
        }
      }

      private void OnEnable()
      {
        XYAxis.action.Enable();
        DashInput.action.Enable();
        DashInput.action.started += OnDashStarted;
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
        DashInput.action.Disable();
        DashInput.action.started -= OnDashStarted;
      }

    }
  }
}