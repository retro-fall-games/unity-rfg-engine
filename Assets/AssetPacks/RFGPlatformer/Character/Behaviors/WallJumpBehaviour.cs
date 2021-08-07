using System.Collections;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Character/Behaviour/Wall Jump Behaviour")]
    public class WallJumpBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public Vector2 wallJumpForce = new Vector2(10f, 4f);
      public bool shouldReduceNumberOfJumpsLeft = true;
      [Header("Audio")]
      public string[] soundFx;
      private JumpBehaviour _jumpBehaviour;
      // private Button _jumpButton;
      private CharacterController2D _controller;
      private CharacterControllerState2D _state;

      public override void InitBehaviour()
      {
        _controller = _character.Controller;
        _state = _character.Controller.State;
        // _jumpBehaviour = _character.FindBehaviour<JumpBehaviour>();
        // StartCoroutine(InitBehaviourCo());
      }

      // private IEnumerator InitBehaviourCo()
      // {
      //   yield return new WaitUntil(() => InputManager.Instance != null);
      //   yield return new WaitUntil(() => InputManager.Instance.JumpButton != null);
      //   _jumpButton = InputManager.Instance.JumpButton;
      //   _jumpButton.State.OnStateChange += JumpButtonOnStateChanged;
      // }

      // private void JumpButtonOnStateChanged(ButtonStates state)
      // {
      //   if (Time.timeScale == 0f || !authorized)
      //   {
      //     return;
      //   }
      //   switch (state)
      //   {
      //     case ButtonStates.Down:
      //       // if (_character.MovementState.CurrentState == MovementStates.WallClinging)
      //       // {
      //       //   WallJump();
      //       // }
      //       break;
      //   }
      // }

      private void WallJump()
      {
        if (soundFx != null && soundFx.Length > 0)
        {
          // FXAudio.Instance.Play(soundFx, false);
        }
        float wallJumpDirection;

        // _character.MovementState.ChangeState(MovementStates.WallJumping);

        if (_jumpBehaviour != null && shouldReduceNumberOfJumpsLeft)
        {
          _jumpBehaviour.SetNumberOfJumpsLeft(_jumpBehaviour.NumberOfJumpsLeft - 1);
        }

        _controller.SlowFall(0f);

        float threshold = 0; // InputManager.Instance.threshold.x;
        float _horizontalInput = 0; //InputManager.Instance.PrimaryMovement.x;
        bool isClingingLeft = _state.IsCollidingLeft && _horizontalInput <= -threshold;
        bool isClingingRight = _state.IsCollidingRight && _horizontalInput >= threshold;

        if (isClingingRight)
        {
          wallJumpDirection = -1f;
        }
        else
        {
          wallJumpDirection = 1f;
        }

        Vector2 wallJumpVector = new Vector2(wallJumpDirection * wallJumpForce.x, Mathf.Sqrt(2f * wallJumpForce.y * Mathf.Abs(_controller.Parameters.Gravity)));

        _controller.AddForce(wallJumpVector);
      }

    }
  }
}