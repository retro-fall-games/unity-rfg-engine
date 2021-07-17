using System;
using UnityEngine;

namespace RFG
{
  public class AIJumpState : TickBaseState
  {
    private Character _character;
    private JumpBehaviour _jumpBehaviour;
    private float jumpTimeElapsed = 0f;
    private float jumpSpeed = 2f;

    public AIJumpState(Character character) : base(character.gameObject)
    {
      _character = character;
      _jumpBehaviour = _character.FindBehaviour<JumpBehaviour>();
    }

    public override Type Tick()
    {
      if (_character.Controller.State.IsJumping == false && _character.Controller.State.IsGrounded)
      {
        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 25)
        {
          return typeof(AIIdleState);
        }
        else if (rand < 75)
        {
          return typeof(AIWanderState);
        }
      }
      if (_character.Controller.State.IsGrounded)
      {
        jumpTimeElapsed += Time.deltaTime;
        if (jumpTimeElapsed >= jumpSpeed)
        {
          jumpTimeElapsed = 0;
          Jump();
        }
      }
      return null;
    }

    public void Jump()
    {
      _jumpBehaviour.JumpStart();
      int rand = UnityEngine.Random.Range(0, 10);
      if (rand < 5)
      {
        // _movement.SetDirection(CharacterMovement.Direction.Left);
      }
      else
      {
        // _movement.SetDirection(CharacterMovement.Direction.Right);
      }
      return;
    }

    public override void OnEnter()
    {
      // _movement.SetDirection(CharacterMovement.Direction.Right);
      Jump();
      return;
    }

    public override void OnExit()
    {
      // _movement.SetDirection(CharacterMovement.Direction.Idle);
      return;
    }
  }
}