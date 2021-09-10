using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Ability/Walking")]
    public class WalkingAbility : MonoBehaviour, IAbility
    {
      [HideInInspector]
      private StateCharacterContext _context;
      private Character _character;
      private CharacterController2D _controller;
      private CharacterControllerState2D _state;
      private InputActionReference _movement;
      private IdleSettings _idleSettings;
      private WalkingSettings _walkingSettings;

      private void Start()
      {
        _character = GetComponent<Character>();
        _context = _character.Context as StateCharacterContext;
        _controller = _context.controller;
        _state = _controller.State;
        _movement = _context.inputPack.Movement;
        _idleSettings = _context.settingsPack.IdleSettings;
        _walkingSettings = _context.settingsPack.WalkingSettings;
      }

      private void Update()
      {
        if (_character.CurrentStateType != typeof(AliveState) || Time.timeScale == 0f)
        {
          return;
        }

        float horizontalSpeed = _movement.action.ReadValue<Vector2>().x;

        if (horizontalSpeed > 0f)
        {
          if (!_state.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }
        else if (horizontalSpeed < 0f)
        {
          if (_state.IsFacingRight && !_controller.rotateOnMouseCursor)
          {
            _controller.Flip();
          }
        }

        // If the movement state is dashing return so it wont get set back to idle
        if (_state.IsDashing)
        {
          return;
        }

        if ((!_state.IsJumping && !_state.IsFalling && _state.IsGrounded) || _state.JustGotGrounded)
        {
          if (horizontalSpeed == 0)
          {
            _state.IsIdle = true;
            _state.IsWalking = false;
            _context.animator.Play(_idleSettings.IdleClip);
          }
          else
          {
            _state.IsIdle = false;
            _state.IsWalking = true;
            _context.transform.SpawnFromPool("Effects", _walkingSettings.WalkingEffects);
            _context.animator.Play(_walkingSettings.WalkingClip);
          }
        }

        float movementFactor = _state.IsGrounded ? _controller.Parameters.GroundSpeedFactor : _controller.Parameters.AirSpeedFactor;
        float movementSpeed = horizontalSpeed * _walkingSettings.WalkingSpeed * _controller.Parameters.SpeedFactor;
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
        if ((_state.IsWalking || _state.IsRunning))
        {
          if ((_state.IsCollidingLeft) || (_state.IsCollidingRight))
          {
            _state.IsWalking = false;
            _state.IsRunning = false;
          }
        }
      }
    }
  }
}