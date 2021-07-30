using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Character/Behaviour/Jump Behaviour")]
  public class JumpBehaviour : PlatformerCharacterBehaviour
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
    public int numberOfJumps = 1;
    public bool canJumpDownOneWayPlatforms = true;

    [Header("Proportional Jumps")]
    public bool jumpIsProportionalToThePressTime = true;
    public float jumpMinAirTime = 0.1f;
    public float jumpReleaseForceFactor = 2f;

    [Header("Audio")]
    public string[] jumpSoundFx;
    public string[] landSoundFx;

    public int NumberOfJumpsLeft { get { return _numberOfJumpsLeft; } }

    [HideInInspector]
    private int _numberOfJumpsLeft = 0;
    private float _lastJumpTime = 0f;
    private Button _jumpButton;

    public override void InitBehaviour()
    {
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => InputManager.Instance != null);
      yield return new WaitUntil(() => InputManager.Instance.JumpButton != null);
      _jumpButton = InputManager.Instance.JumpButton;
      _jumpButton.State.OnStateChange += JumpButtonOnStateChanged;
    }

    private void JumpButtonOnStateChanged(ButtonStates state)
    {
      if (Time.timeScale == 0f)
      {
        return;
      }
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

    public override void ProcessBehaviour()
    {
      // Reset the number of jumps back because just got grounded
      if (_character.Controller.State.JustGotGrounded)
      {
        if (landSoundFx != null && landSoundFx.Length > 0)
        {
          FXAudio.Instance.Play(landSoundFx, false);
        }
        _numberOfJumpsLeft = numberOfJumps;
      }
    }

    public void JumpStart()
    {
      if (!CanJump())
      {
        return;
      }

      if (jumpSoundFx != null && jumpSoundFx.Length > 0)
      {
        FXAudio.Instance.Play(jumpSoundFx, false);
      }

      _character.Controller.CollisionsOnStairs(true);

      float _verticalInput = InputManager.Instance.PrimaryMovement.y;

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
      if (jumpIsProportionalToThePressTime)
      {
        bool hasMinAirTime = Time.time - _lastJumpTime >= jumpMinAirTime;
        bool speedGreaterThanGravity = _character.Controller.Velocity.y > Mathf.Sqrt(Mathf.Abs(_character.Controller.Parameters.gravity));
        if (hasMinAirTime && speedGreaterThanGravity)
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

      if (_character.MovementState.CurrentState == MovementStates.WallClinging)
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