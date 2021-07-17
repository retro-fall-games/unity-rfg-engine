using System;
using UnityEngine;

namespace RFG
{
  public class AIWanderState : TickBaseState
  {
    private Character _character;
    private float decisionTimeElapsed = 0f;
    private float decisionSpeed = 3f;

    public AIWanderState(Character character) : base(character.gameObject)
    {
      _character = character;
    }

    public override Type Tick()
    {
      if (_character.characterType == CharacterType.AI)
      {
        decisionTimeElapsed += Time.deltaTime;
        if (decisionTimeElapsed >= decisionSpeed)
        {
          decisionTimeElapsed = 0;
          return MakeDecision();
        }
      }

      return null;
    }

    public override void OnEnter()
    {
      int rand = UnityEngine.Random.Range(0, 100);
      if (rand < 50)
      {
        // _movement.SetDirection(CharacterMovement.Direction.Left);
      }
      else if (rand < 100)
      {
        // _movement.SetDirection(CharacterMovement.Direction.Right);
      }
      return;
    }
    public override void OnExit()
    {
      return;
    }

    public Type MakeDecision()
    {
      int rand = UnityEngine.Random.Range(0, 100);
      if (rand < 50)
      {
        return typeof(AIIdleState);
      }
      else if (rand < 75)
      {
        return typeof(AIJumpState);
      }
      return null;
    }
  }
}