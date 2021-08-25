using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/AI Behaviours/AI Movement Path")]
    public class AIMovementPathBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      /// <summary>Walking Settings to know speed and effects</summary>
      [Tooltip("Walking Settings to know speed and effects")]
      public WalkingSettings WalkingSettings;
      public MovementPath MovementPath;

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
        if (_character.CurrentStateType == typeof(AIMovementPathState))
        {
          _controller.RotateTowards(MovementPath.NextPath);
          if (!MovementPath.autoMove)
          {
            MovementPath.Move();
            MovementPath.CheckPath();
          }

          if (MovementPath.state == MovementPath.State.OneWay && MovementPath.ReachedEnd)
          {
            _character.ChangeState(typeof(AIIdleState));
          }
          else
          {
            _character.ChangeState(typeof(AIMovementPathState));
          }
        }

      }

    }
  }
}