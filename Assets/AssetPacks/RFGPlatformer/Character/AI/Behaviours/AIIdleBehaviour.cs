using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Idle")]
    public class AIIdleBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>AI Idle Settings to know effects</summary>
      [Tooltip("AI Idle Settings to know effects")]
      public AIIdleSettings AIIdleSettings;

      [HideInInspector]
      private Transform _transform;
      private Character _character;
      private CharacterController2D _controller;
      private Animator _animator;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
      }

      private void LateUpdate()
      {
        if (_character.CurrentStateType == typeof(AIIdleState))
        {
          _transform.SpawnFromPool("Effects", AIIdleSettings.IdleEffects);
          _animator.Play(AIIdleSettings.IdleClip);
          _controller.SetHorizontalForce(0);
        }
      }

    }
  }
}