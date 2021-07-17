using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/Dash Behaviour")]
  public class DashBehaviour : CharacterBehaviour
  {

    [Header("Dash")]
    public float dashDistance = 3f;
    public float dashForce = 40f;
    public int totalDashes = 2;
    public int numberOfDashesLeft = 2;
    [Header("Direction")]
    public Aim aim;
    public float minInputThreshold = 0.1f;

    [Header("Cooldown")]
    public float cooldown = 1f;

    private Vector2 _dashDirection;
    private float _distanceTraveled = 0f;
    private bool _shouldKeepDashing = true;
    private Vector3 _initialPosition;
    private Button _dashButton;
    private float _cooldownTimestamp;
    private IEnumerator _dashCoroutine;
    private float _lastDashAt = 0f;

    public override void InitBehaviour()
    {
      aim.Init();
      numberOfDashesLeft = totalDashes;
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput != null);
      yield return new WaitUntil(() => _character.CharacterInput.DashButton != null);
      _dashButton = _character.CharacterInput.DashButton;
      _dashButton.State.OnStateChange += DashButtonOnStateChanged;
    }

    public override void ProcessBehaviour()
    {
      if (_character.MovementState.CurrentState == MovementStates.Dashing)
      {
        _character.Controller.GravityActive(false);
      }
      HandleAmountOfDashesLeft();
    }

    private void DashButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          StartDash();
          break;
      }
    }

    private void StartDash()
    {
      if (_cooldownTimestamp > Time.time)
      {
        return;
      }

      if (numberOfDashesLeft <= 0)
      {
        return;
      }

      _character.Controller.CollisionsOnStairs(true);
      _character.MovementState.ChangeState(MovementStates.Dashing);
      _cooldownTimestamp = Time.time + cooldown;
      _distanceTraveled = 0f;
      _shouldKeepDashing = true;
      _initialPosition = _transform.position;
      _lastDashAt = Time.time;

      numberOfDashesLeft--;

      ComputerDashDirection();
      CheckFlipCharacter();

      _dashCoroutine = Dash();
      StartCoroutine(_dashCoroutine);
    }

    private void ComputerDashDirection()
    {
      if (_character.CharacterInput != null && _character.CharacterInput.InputManager != null)
      {
        aim.PrimaryMovement = _character.CharacterInput.InputManager.PrimaryMovement;
      }
      aim.CurrentPosition = _transform.position;
      _dashDirection = aim.GetCurrentAim();

      CheckAutoCorrectTrajectory();

      if (_dashDirection.magnitude < minInputThreshold)
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
      while (_distanceTraveled < dashDistance && _shouldKeepDashing && _character.MovementState.CurrentState == MovementStates.Dashing)
      {
        _distanceTraveled = Vector3.Distance(_initialPosition, transform.position);

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
          _character.Controller.SetForce(_dashDirection * dashForce);
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
      _character.Controller.GravityActive(true);

      _character.Controller.SetForce(Vector2.zero);

      if (_character.MovementState.CurrentState == MovementStates.Dashing)
      {
        if (_character.Controller.State.IsGrounded)
        {
          _character.MovementState.ChangeState(MovementStates.Idle);
        }
        else
        {
          _character.MovementState.RestorePreviousState();
        }
      }

    }

    public void SetNumberOfDashesLeft(int numberLeft)
    {
      numberOfDashesLeft = numberLeft;
    }

    private void HandleAmountOfDashesLeft()
    {
      if (Time.time - _lastDashAt < cooldown)
      {
        return;
      }

      if (_character.Controller.State.IsGrounded)
      {
        SetNumberOfDashesLeft(totalDashes);
      }
    }

  }
}