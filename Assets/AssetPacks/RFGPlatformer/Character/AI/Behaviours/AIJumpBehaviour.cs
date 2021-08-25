using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Jump")]
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
      private AIBrainBehaviour _brain;

      public float JumpSpeed = 5f;
      private float _jumpTimeElapsed = 0f;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
        _aggro = GetComponent<Aggro>();
        _animator = GetComponent<Animator>();
        _brain = GetComponent<AIBrainBehaviour>();
      }

      private void LateUpdate()
      {
        if (_character.CurrentStateType == typeof(AIJumpingState))
        {
          if (_controller.State.JustGotGrounded)
          {
            _controller.SetHorizontalForce(0);
            _transform.SpawnFromPool("Effects", JumpSettings.LandEffects);
            _brain.RestorePreviousDecision();
          }
          if (_controller.State.IsGrounded)
          {
            _jumpTimeElapsed += Time.deltaTime;
            if (_jumpTimeElapsed >= JumpSpeed)
            {
              _jumpTimeElapsed = 0;
              JumpStart();
            }
          }
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
        float movementSpeed = _normalizedHorizontalSpeed * speed * _controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        _controller.SetHorizontalForce(horizontalMovementForce);

        JumpStop();
      }

      private void JumpStop()
      {
        _controller.State.IsFalling = true;
        _controller.State.IsJumping = false;
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