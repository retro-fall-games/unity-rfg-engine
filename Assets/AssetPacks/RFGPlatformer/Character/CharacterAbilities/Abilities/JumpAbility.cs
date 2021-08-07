using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Jump")]
    public class JumpAbility : CharacterAbility
    {
      public enum JumpRestrictions
      {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
      }

      [Header("Sound FX")]
      public SoundData[] JumpFx;
      public SoundData[] LandFx;

      [Header("Jump Parameters")]
      public float JumpHeight = 12f;
      public float OneWayPlatformFallVelocity = -10f;

      [Header("Jump Restrictions")]
      public JumpRestrictions Restrictions;
      public int NumberOfJumps = 1;
      public int NumberOfJumpsLeft { get { return _numberOfJumpsLeft; } }
      public bool CanJumpDownOneWayPlatforms = true;

      [Header("Proportional Jumps")]
      public bool JumpIsProportionalToThePressTime = true;
      public float JumpMinAirTime = 0.1f;
      public float JumpReleaseForceFactor = 2f;

      [HideInInspector]
      private int _numberOfJumpsLeft = 0;
      private float _lastJumpTime = 0f;
      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;

      public override void Init(Character character)
      {
        _character = character;
        _controller = character.Controller;
        _movement = character.Input.Movement;
      }

      public override void EarlyProcess()
      {
        _movementVector = _movement.ReadValue<Vector2>();
      }

      public override void Process()
      {
        if (_controller.State.JustGotGrounded)
        {
          if (LandFx.Length > 0)
          {
            SoundManager.Instance.Play(LandFx);
          }
          _numberOfJumpsLeft = NumberOfJumps;
        }
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
        JumpStart();
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
        JumpStop();
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }

      public void JumpStart()
      {
        if (!CanJump())
        {
          return;
        }

        if (JumpFx.Length > 0)
        {
          SoundManager.Instance.Play(JumpFx);
        }

        _controller.CollisionsOnStairs(true);

        float _verticalInput = _movementVector.y;

        if (_verticalInput < 0f)
        {
          _lastJumpTime = Time.time;
          _controller.State.IsFalling = true;
          _character.MovementState = MovementState.Falling;
          _controller.IgnoreOneWayPlatformsThisFrame = true;
          _controller.SetVerticalForce(OneWayPlatformFallVelocity);
          _controller.IgnoreStairsForTime(0.1f);
        }
        else
        {
          _lastJumpTime = Time.time;
          _controller.State.IsFalling = false;
          _controller.State.IsJumping = true;
          _character.MovementState = MovementState.Jumping;
          _numberOfJumpsLeft--;
          _controller.AddVerticalForce(Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
        }

      }

      private void JumpStop()
      {
        if (JumpIsProportionalToThePressTime)
        {
          bool hasMinAirTime = Time.time - _lastJumpTime >= JumpMinAirTime;
          bool speedGreaterThanGravity = _controller.Velocity.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity));
          if (hasMinAirTime && speedGreaterThanGravity)
          {
            _lastJumpTime = 0f;
            if (JumpReleaseForceFactor == 0f)
            {
              _controller.SetVerticalForce(0f);
            }
            else
            {
              _controller.AddVerticalForce(-_controller.Velocity.y / JumpReleaseForceFactor);
            }
          }
        }
        _controller.State.IsFalling = true;
        _character.MovementState = MovementState.Falling;
      }

      private bool CanJump()
      {
        if (Restrictions == JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }

        if (Restrictions == JumpRestrictions.CanJumpOnGround && _numberOfJumpsLeft <= 0)
        {
          return false;
        }

        if (_character.MovementState == MovementState.WallClinging)
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
}