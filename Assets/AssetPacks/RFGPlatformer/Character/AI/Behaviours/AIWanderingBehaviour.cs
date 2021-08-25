using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Wandering")]
    public class AIWanderingBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>Walking Settings to know speed and effects</summary>
      [Tooltip("Walking Settings to know speed and effects")]
      public WalkingSettings WalkingSettings;
      public float ChangeDirectionSpeed = 5f;
      private float _changeDirectionTimeElapsed = 0f;

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

      private void Update()
      {
        if (_character.CurrentStateType == typeof(AIWanderingState))
        {
          Flip();

          float _normalizedHorizontalSpeed = 0f;

          if (_controller.State.IsFacingRight)
          {
            _normalizedHorizontalSpeed = 1f;
          }
          else
          {
            _normalizedHorizontalSpeed = -1f;
          }

          _transform.SpawnFromPool("Effects", WalkingSettings.WalkingEffects);
          _animator.Play(WalkingSettings.WalkingClip);

          float movementFactor = _controller.Parameters.GroundSpeedFactor;
          float movementSpeed = _normalizedHorizontalSpeed * WalkingSettings.WalkingSpeed * _controller.Parameters.SpeedFactor;
          float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

          _controller.SetHorizontalForce(horizontalMovementForce);
        }
      }

      private void Flip()
      {
        if (_controller.State.IsCollidingLeft || _controller.State.IsCollidingRight)
        {
          _controller.Flip();
          _changeDirectionTimeElapsed = 0;
        }
        else
        {
          _changeDirectionTimeElapsed += Time.deltaTime;
          if (_changeDirectionTimeElapsed >= ChangeDirectionSpeed)
          {
            _changeDirectionTimeElapsed = 0;
            int decisionIndex = UnityEngine.Random.Range(0, 100);
            if (decisionIndex >= 50)
            {
              _controller.Flip();
            }
          }
        }
      }

    }
  }
}