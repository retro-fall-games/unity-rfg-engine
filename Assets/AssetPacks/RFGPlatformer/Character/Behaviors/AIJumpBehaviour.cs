using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Character/Behaviour/AI Jump Behaviour")]
    public class AIJumpBehaviour : CharacterBehaviour
    {
      public enum JumpRestrictions
      {
        CanJumpOnGround,
        CanJumpAnywhere,
        CantJump
      }

      [Header("Jump Parameters")]
      public float jumpHeight = 12f;
      public float horizontalSpeed = 3f;

      [Header("Jump Restrictions")]
      public JumpRestrictions jumpRestrictions;

      [Header("Audio FX")]
      public AudioSource[] jumpAudio;
      public AudioSource[] landAudio;

      public override void ProcessBehaviour()
      {
        if (_character.Controller.State.JustGotGrounded)
        {
          if (landAudio != null && landAudio.Length > 0)
          {
            landAudio.PlayAll();
          }
        }
        // if (_character.AIMovementState.CurrentState == AIMovementStates.JumpingLeft || _character.AIMovementState.CurrentState == AIMovementStates.JumpingRight)
        // {
        //   JumpStart();
        // }
      }

      public void JumpStart()
      {
        if (!CanJump())
        {
          return;
        }

        if (jumpAudio != null && jumpAudio.Length > 0)
        {
          jumpAudio.PlayAll();
        }

        // if (_character.AIMovementState.CurrentState == AIMovementStates.JumpingLeft && _character.Controller.State.IsFacingRight)
        // {
        //   _character.Controller.Flip();
        // }
        // else if (_character.AIMovementState.CurrentState == AIMovementStates.JumpingRight && !_character.Controller.State.IsFacingRight)
        // {
        //   _character.Controller.Flip();
        // }

        // // Jump
        // _character.Controller.CollisionsOnStairs(true);
        // _character.Controller.State.IsFalling = false;
        // _character.Controller.State.IsJumping = true;
        // _character.MovementState.ChangeState(MovementStates.Jumping);
        // _character.Controller.AddVerticalForce(Mathf.Sqrt(2f * jumpHeight * Mathf.Abs(_character.Controller.Parameters.Gravity)));

        // // Move horizontally
        // float _normalizedHorizontalSpeed = 0f;
        // if (_character.Controller.State.IsFacingRight)
        // {
        //   _normalizedHorizontalSpeed = 1f;
        // }
        // else
        // {
        //   _normalizedHorizontalSpeed = -1f;
        // }

        // float movementFactor = _character.Controller.Parameters.AirSpeedFactor;
        // float movementSpeed = _normalizedHorizontalSpeed * horizontalSpeed * _character.Controller.Parameters.SpeedFactor;
        // float horizontalMovementForce = Mathf.Lerp(_character.Controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        // _character.Controller.SetHorizontalForce(horizontalMovementForce);

        // JumpStop();
      }

      private void JumpStop()
      {
        // _character.Controller.State.IsFalling = true;
        // _character.MovementState.ChangeState(MovementStates.Falling);
        // _character.AIMovementState.RestorePreviousState();
      }

      private bool CanJump()
      {
        if (jumpRestrictions == JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }
        if (jumpRestrictions == JumpRestrictions.CanJumpOnGround && _character.Controller.State.IsGrounded)
        {
          return true;
        }
        return false;
      }

    }
  }
}