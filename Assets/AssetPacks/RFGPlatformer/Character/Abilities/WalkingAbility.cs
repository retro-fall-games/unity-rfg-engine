using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Walking")]
    public class WalkingAbility : MonoBehaviour, IAbility
    {
      [Header("Input")]
      /// <summary>Input Action to read the xy axis</summary>
      [Tooltip("Input Action to read the xy axis")]
      public InputActionReference XYAxis;

      [Header("Settings")]
      /// <summary>Idle Settings for animations</summary>
      [Tooltip("Idle Settings for animations")]
      public IdleSettings IdleSettings;

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

        if ((!_controller.State.IsJumping && !_controller.State.IsFalling && _controller.State.IsGrounded) || _controller.State.JustGotGrounded)
        {
          if (horizontalSpeed == 0)
          {
            _controller.State.IsIdle = true;
            _controller.State.IsWalking = false;
            _animator.Play(IdleSettings.IdleClip);
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
        float horizontalMovementForce = Mathf.Lerp(_controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

        // add any external forces that may be active right now
        // if (Mathf.Abs(_controller.ExternalForce.x) > 0)
        // {
        //   horizontalMovementForce += _controller.ExternalForce.x;
        // }

        // we handle friction
        horizontalMovementForce = HandleFriction(horizontalMovementForce);

        _controller.SetHorizontalForce(horizontalMovementForce);

        DetectWalls();
      }

      /// <summary>
      /// Handles surface friction.
      /// </summary>
      /// <returns>The modified current force.</returns>
      /// <param name="force">the force we want to apply friction to.</param>
      protected virtual float HandleFriction(float force)
      {
        // if we have a friction above 1 (mud, water, stuff like that), we divide our speed by that friction
        if (_controller.Friction > 1)
        {
          force = force / _controller.Friction;
        }

        // if we have a low friction (ice, marbles...) we lerp the speed accordingly
        if (_controller.Friction < 1 && _controller.Friction > 0)
        {
          force = Mathf.Lerp(_controller.Speed.x, force, Time.deltaTime * _controller.Friction * 10);
        }

        return force;
      }

      protected virtual void DetectWalls()
      {
        if ((_controller.State.IsWalking || _controller.State.IsRunning))
        {
          if ((_controller.State.IsCollidingLeft) || (_controller.State.IsCollidingRight))
          {
            _controller.State.IsWalking = false;
            _controller.State.IsRunning = false;
          }
        }
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