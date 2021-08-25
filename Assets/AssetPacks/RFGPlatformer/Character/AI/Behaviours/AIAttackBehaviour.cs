using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Attack")]
    public class AIAttackBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>AI Attack Settings to know speed and effects</summary>
      [Tooltip("AI Attack Settings to know speed and effects")]
      public AIAttackSettings AIAttackSettings;

      /// <summary>Running Settings to know how fast to move horizontally</summary>
      [Tooltip("Running Settings to know how fast to move horizontally when aggro is true")]
      public RunningSettings RunningSettings;

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
        if (_character.CurrentStateType == typeof(AIAttackingState))
        {
          // Rotate to always face the target
          _controller.RotateTowards(_aggro.target2);

          // Move towards that target
          float _normalizedHorizontalSpeed = 0f;

          if (_controller.State.IsFacingRight)
          {
            _normalizedHorizontalSpeed = 1f;
          }
          else
          {
            _normalizedHorizontalSpeed = -1f;
          }

          _transform.SpawnFromPool("Effects", RunningSettings.RunningEffects);
          _animator.Play(RunningSettings.RunningClip);

          float movementFactor = _controller.Parameters.GroundSpeedFactor;
          float movementSpeed = _normalizedHorizontalSpeed * RunningSettings.RunningSpeed * _controller.Parameters.SpeedFactor;
          float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

          _controller.SetHorizontalForce(horizontalMovementForce);

          //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
          //   {
          //     ctx.equipmentSet.PrimaryWeapon?.Perform();
          //   }

          //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
          //   {
          //     ctx.equipmentSet.SecondaryWeapon?.Perform();
          //   }
        }
      }
    }
  }
}