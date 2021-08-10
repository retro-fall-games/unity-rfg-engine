using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Wall Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Wall Jump")]
    public class WallJumpAbility : CharacterAbility
    {
      [Header("Settings")]
      public float Threshold = 0.01f;
      public Vector2 WallJumpForce = new Vector2(10f, 4f);

      [Header("Sound FX")]
      public SoundData[] JumpFx;

      private Character _character;
      private CharacterController2D _controller;
      private InputAction _movement;
      private Vector2 _movementVector;

      public override void Init(Character character)
      {
        _character = character;
        _controller = character.Controller;
        _movement = character.Input.Movement;
      }

      public override void EarlyProcess()
      {
        _movementVector = _movement.ReadValue<Vector2>();
      }

      public override void Process()
      {
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
        if (_character.CharacterMovementState.CurrentStateType == typeof(WallClingingState))
        {
          WallJump();
        }
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }

      private void WallJump()
      {
        if (JumpFx.Length > 0)
        {
          SoundManager.Instance.Play(JumpFx);
        }
        float wallJumpDirection;

        _character.CharacterMovementState.ChangeState(typeof(WallJumpingState));

        _controller.SlowFall(0f);

        float _horizontalInput = _movementVector.x;
        bool isClingingLeft = _controller.State.IsCollidingLeft && _horizontalInput <= -Threshold;
        bool isClingingRight = _controller.State.IsCollidingRight && _horizontalInput >= Threshold;

        if (isClingingRight)
        {
          wallJumpDirection = -1f;
        }
        else
        {
          wallJumpDirection = 1f;
        }

        Vector2 wallJumpVector = new Vector2(wallJumpDirection * WallJumpForce.x, Mathf.Sqrt(2f * WallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity)));

        _controller.AddForce(wallJumpVector);
      }

    }
  }
}