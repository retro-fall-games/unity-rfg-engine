using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behavior/Jump Behavior")]
  public class JumpBehavior : CharacterBehavior
  {
    public enum JumpRestrictions
    {
      CanJumpOnGround,
      CanJumpAnywhere,
      CantJump
    }

    [Header("Jump Parameters")]
    public float jumpHeight = 12f;
    public float oneWayPlatformFallVelocity = -10f;

    [Header("Jump Restrictions")]
    public JumpRestrictions jumpRestrictions;
    public int numberOfJumps = 2;
    public bool canJumpDownOneWayPlatforms = true;

    [Header("Proportional Jumps")]
    public bool jumpIsProportionalToThePressTime = true;
    public float jumpMinAirTime = 0.1f;
    public float jumpReleaseForceFactor = 2f;

    public int NumberOfJumpsLeft { get { return _numberOfJumpsLeft; } }

    [HideInInspector]
    private int _numberOfJumpsLeft = 0;
    private float _lastJumpTime = 0f;
    private Button _jumpButton;

    public override void InitBehavior()
    {
      StartCoroutine(InitBehaviorCo());
    }

    private IEnumerator InitBehaviorCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput != null);
      yield return new WaitUntil(() => _character.CharacterInput.JumpButton != null);
      _jumpButton = _character.CharacterInput.JumpButton;
      _jumpButton.State.OnStateChange += JumpButtonOnStateChanged;
    }

    private void JumpButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          JumpStart();
          break;
        case ButtonStates.Up:
          JumpStop();
          break;
      }
    }

    public override void ProcessBehavior()
    {
      // Reset the number of jumps back because just got grounded
      if (_character.Controller.State.JustGotGrounded)
      {
        _numberOfJumpsLeft = numberOfJumps;
      }
    }

    private void JumpStart()
    {
      if (!CanJump())
      {
        return;
      }

      _character.Controller.CollisionsOnStairs(true);

      if (_verticalInput < 0f)
      {
        _lastJumpTime = Time.time;
        _character.Controller.State.IsFalling = true;
        _character.MovementState.ChangeState(MovementStates.Falling);
        _character.Controller.IgnoreOneWayPlatformsThisFrame = true;
        _character.Controller.SetVerticalForce(oneWayPlatformFallVelocity);
        _character.Controller.IgnoreStairsForTime(0.1f);
      }
      else
      {
        _lastJumpTime = Time.time;
        _character.Controller.State.IsFalling = false;
        _character.Controller.State.IsJumping = true;
        _character.MovementState.ChangeState(MovementStates.Jumping);
        _numberOfJumpsLeft--;
        _character.Controller.AddVerticalForce(Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(_character.Controller.Parameters.gravity)));
      }

    }

    private void JumpStop()
    {
      bool hasMinAirTime = Time.time - _lastJumpTime >= jumpMinAirTime;
      bool speedGreaterThanGravity = _character.Controller.Velocity.y > Mathf.Sqrt(Mathf.Abs(_character.Controller.Parameters.gravity));
      if (hasMinAirTime && speedGreaterThanGravity && jumpIsProportionalToThePressTime)
      {
        _lastJumpTime = 0f;
        if (jumpReleaseForceFactor == 0f)
        {
          _character.Controller.SetVerticalForce(0f);
        }
        else
        {
          _character.Controller.AddVerticalForce(-_character.Controller.Velocity.y / jumpReleaseForceFactor);
        }
      }
      _character.Controller.State.IsFalling = true;
      _character.MovementState.ChangeState(MovementStates.Falling);
    }

    private bool CanJump()
    {
      if (jumpRestrictions == JumpRestrictions.CanJumpAnywhere)
      {
        return true;
      }

      if (jumpRestrictions == JumpRestrictions.CanJumpOnGround && _numberOfJumpsLeft <= 0)
      {
        return false;
      }

      return true;
    }

    public void SetNumberOfJumpsLeft(int numberLeft)
    {
      _numberOfJumpsLeft = numberLeft;
    }

  }
}