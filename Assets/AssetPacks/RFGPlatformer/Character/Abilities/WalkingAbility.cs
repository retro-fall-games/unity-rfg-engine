using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Walking")]
    public class WalkingAbility : MonoBehaviour
    {
      [Header("Input")]
      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      [Header("Settings")]
      /// <summary>Walking Settings to know if there is any input and changing state</summary>
      [Tooltip("Walking Settings to know if there is any input and changing state")]
      public WalkingSettings WalkingSettings;

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

      private void Update()
      {
        if (_character.CurrentStateType != typeof(AliveState) || Time.timeScale == 0f)
        {
          return;
        }

        float horizontalSpeed = XYAxis.action.ReadValue<Vector2>().x;
        // float _verticalInput = inputMovement.y;

        if (horizontalSpeed > 0f)
        {
          if (!_controller.State.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }
        else if (horizontalSpeed < 0f)
        {
          if (_controller.State.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }

        // If the movement state is dashing return so it wont get set back to idle
        if (_controller.State.IsDashing)
        {
          return;
        }

        // if (_verticalInput >= 1 || _verticalInput <= -1)
        // {
        //   _controller.CollisionsOnStairs(true);
        // }


        if ((!_controller.State.IsJumping && !_controller.State.IsFalling && _controller.State.IsGrounded) || _controller.State.JustGotGrounded)
        {
          if (horizontalSpeed == 0)
          {
            _controller.State.IsIdle = true;
            _controller.State.IsWalking = false;
            _animator.Play(WalkingSettings.IdleClip);
          }
          else
          {
            _controller.State.IsIdle = false;
            _controller.State.IsWalking = true;
            _transform.SpawnFromPool("Effects", WalkingSettings.WalkingEffects);
            _animator.Play(WalkingSettings.WalkingClip);
          }
        }

        float movementFactor = _controller.State.IsGrounded ? _controller.Parameters.GroundSpeedFactor : _controller.Parameters.AirSpeedFactor;
        float movementSpeed = horizontalSpeed * WalkingSettings.WalkingSpeed * _controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(_controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);
        _controller.SetHorizontalForce(horizontalMovementForce);
      }

      private void OnEnable()
      {
        XYAxis.action.Enable();
      }

      private void OnDisable()
      {
        XYAxis.action.Disable();
      }

    }
  }
}