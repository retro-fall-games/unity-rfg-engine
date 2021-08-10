using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dash Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Dash")]
    public class DashAbility : CharacterAbility
    {
      [Header("Dash")]
      public float DashDistance = 3f;
      public float DashForce = 40f;
      public int TotalDashes = 2;
      public int NumberOfDashesLeft = 2;

      [Header("Direction")]
      public Aim aim;
      public float MinInputThreshold = 0.1f;

      [Header("Cooldown")]
      public float Cooldown = 1f;

      [Header("Sound FX")]
      public SoundData[] DashFx;

      private Vector2 _dashDirection;
      private float _distanceTraveled = 0f;
      private bool _shouldKeepDashing = true;
      private Vector3 _initialPosition;
      private float _cooldownTimestamp;
      private IEnumerator _dashCoroutine;
      private float _lastDashAt = 0f;
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;

      public override void Init(Character character)
      {
        _character = character;
        _transform = character.transform;
        _controller = character.Controller;
        _movement = character.Input.Movement;
        _cooldownTimestamp = 0;
        NumberOfDashesLeft = TotalDashes;
        aim.Init();
      }

      public override void EarlyProcess()
      {
        _movementVector = _movement.ReadValue<Vector2>();
      }

      public override void Process()
      {
        if (_character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          _character.Controller.GravityActive(false);
        }
        HandleAmountOfDashesLeft();
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
        StartDash();
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }

      private void StartDash()
      {
        Debug.Log("Got here 1");
        Debug.Log("_cooldownTimestamp: " + _cooldownTimestamp);
        Debug.Log("Time.time: " + Time.time);
        if (_cooldownTimestamp > Time.time)
        {
          return;
        }

        Debug.Log("Got here 2");
        if (NumberOfDashesLeft <= 0)
        {
          return;
        }

        if (DashFx.Length > 0)
        {
          SoundManager.Instance.Play(DashFx);
        }

        Debug.Log("Got here 3");

        _character.Controller.CollisionsOnStairs(true);
        _character.CharacterMovementState.ChangeState(typeof(DashingState));
        _cooldownTimestamp = Time.time + Cooldown;
        _distanceTraveled = 0f;
        _shouldKeepDashing = true;
        _initialPosition = _transform.position;
        _lastDashAt = Time.time;

        NumberOfDashesLeft--;

        ComputerDashDirection();
        CheckFlipCharacter();

        _dashCoroutine = Dash();
        GameManager.Instance.StartCoroutine(_dashCoroutine);
      }

      private void ComputerDashDirection()
      {
        aim.PrimaryMovement = _movementVector;
        aim.CurrentPosition = _transform.position;
        _dashDirection = aim.GetCurrentAim();

        CheckAutoCorrectTrajectory();

        if (_dashDirection.magnitude < MinInputThreshold)
        {
          _dashDirection = _character.Controller.State.IsFacingRight ? Vector2.right : Vector2.left;
        }
        else
        {
          _dashDirection = _dashDirection.normalized;
        }
      }

      private void CheckAutoCorrectTrajectory()
      {
        if (_character.Controller.State.IsCollidingBelow && _dashDirection.y < 0f)
        {
          _dashDirection.y = 0f;
        }
      }

      private void CheckFlipCharacter()
      {
        if (Mathf.Abs(_dashDirection.x) > 0.05f)
        {
          if (_character.Controller.State.IsFacingRight != _dashDirection.x > 0f)
          {
            _character.Controller.Flip();
          }
        }
      }

      private IEnumerator Dash()
      {
        Debug.Log("Dash");
        while (_distanceTraveled < DashDistance && _shouldKeepDashing && _character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          _distanceTraveled = Vector3.Distance(_initialPosition, _transform.position);

          if ((_character.Controller.State.IsCollidingLeft && _dashDirection.x < 0f)
            || (_character.Controller.State.IsCollidingRight && _dashDirection.x > 0f)
            || (_character.Controller.State.IsCollidingAbove && _dashDirection.y > 0f)
            || (_character.Controller.State.IsCollidingBelow && _dashDirection.y < 0f))
          {
            _shouldKeepDashing = false;
            _character.Controller.SetForce(Vector2.zero);
          }
          else
          {
            _character.Controller.GravityActive(false);
            _character.Controller.SetForce(_dashDirection * DashForce);
          }
          yield return null;
        }
        StopDash();
      }

      private void StopDash()
      {
        if (_dashCoroutine != null)
        {
          GameManager.Instance.StopCoroutine(_dashCoroutine);
        }
        _character.Controller.GravityActive(true);

        _character.Controller.SetForce(Vector2.zero);

        if (_character.CharacterMovementState.CurrentStateType == typeof(DashingState))
        {
          if (_character.Controller.State.IsGrounded)
          {
            _character.CharacterMovementState.ChangeState(typeof(IdleState));
          }
          else
          {
            _character.CharacterMovementState.RestorePreviousState();
          }
        }

      }

      public void SetNumberOfDashesLeft(int numberLeft)
      {
        NumberOfDashesLeft = numberLeft;
      }

      private void HandleAmountOfDashesLeft()
      {
        if (Time.time - _lastDashAt < Cooldown)
        {
          return;
        }

        if (_character.Controller.State.IsGrounded)
        {
          SetNumberOfDashesLeft(TotalDashes);
        }
      }

    }
  }
}