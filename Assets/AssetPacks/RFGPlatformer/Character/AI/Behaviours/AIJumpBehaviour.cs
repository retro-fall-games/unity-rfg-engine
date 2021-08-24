using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/AI Jump")]
    public class AIJumpBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>Walking Settings to know how fast to move horizontally</summary>
      [Tooltip("Walking Settings to know how fast to move horizontally when aggro is false")]
      public WalkingSettings WalkingSettings;

      /// <summary>Running Settings to know how fast to move horizontally</summary>
      [Tooltip("Running Settings to know how fast to move horizontally when aggro is true")]
      public RunningSettings RunningSettings;

      /// <summary>Jump Settings to know how many jumps left and jump restrictions</summary>
      [Tooltip("Jump Settings to know how many jumps left and jump restrictions")]
      public JumpSettings JumpSettings;

      [HideInInspector]
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private Aggro _aggro;
      private Animator _animator;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
        _aggro = GetComponent<Aggro>();
        _animator = GetComponent<Animator>();
      }

      private void LateUpdate()
      {
        if (_controller.State.JustGotGrounded)
        {
          _transform.SpawnFromPool("Effects", JumpSettings.LandEffects);
        }
        if (_character.CurrentStateType == typeof(AIJumpingState))
        {
          JumpStart();
        }
      }

      public void JumpStart()
      {
        if (!CanJump())
        {
          return;
        }

        _transform.SpawnFromPool("Effects", JumpSettings.JumpEffects);
        _animator.Play(JumpSettings.JumpingClip);

        // if (ctx.character.CurrentStateType == typeof(AIJumpingLeftState) && _controller.State.IsFacingRight)
        // {
        //   _controller.Flip();
        // }
        // else if (ctx.character.CurrentStateType == typeof(AIJumpingRightState) && !_controller.State.IsFacingRight)
        // {
        //   _controller.Flip();
        // }

        // Jump
        _controller.CollisionsOnStairs(true);
        _controller.State.IsFalling = false;
        _controller.State.IsJumping = true;
        _controller.AddVerticalForce(Mathf.Sqrt(2f * JumpSettings.JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));

        // Move horizontally
        float _normalizedHorizontalSpeed = 0f;
        if (_controller.State.IsFacingRight)
        {
          _normalizedHorizontalSpeed = 1f;
        }
        else
        {
          _normalizedHorizontalSpeed = -1f;
        }

        float speed = WalkingSettings.WalkingSpeed;
        if (_aggro != null && _aggro.HasAggro)
        {
          speed = RunningSettings.RunningSpeed;
        }

        float movementFactor = _controller.Parameters.AirSpeedFactor;
        float movementSpeed = _normalizedHorizontalSpeed * WalkingSettings.WalkingSpeed * _controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        _controller.SetHorizontalForce(horizontalMovementForce);

        JumpStop();
      }

      private void JumpStop()
      {
        _controller.State.IsFalling = true;
        _character.RestorePreviousState();
      }

      private bool CanJump()
      {
        if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpAnywhere)
        {
          return true;
        }
        if (JumpSettings.Restrictions == JumpSettings.JumpRestrictions.CanJumpOnGround && _controller.State.IsGrounded)
        {
          return true;
        }
        return false;
      }

    }
  }
}