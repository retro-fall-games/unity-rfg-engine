using System;
using UnityEngine;

namespace RFG
{
  public class AIMovementPathState : TickBaseState
  {
    private Character _character;
    private MovementPath _movementPath;

    public AIMovementPathState(Character character, MovementPath movementPath) : base(character.gameObject)
    {
      _character = character;
      _movementPath = movementPath;
    }

    public override Type Tick()
    {
      Transform target = _movementPath.NextPath;
      _character.Controller.RotateTowards(target);
      _movementPath.CheckPath();
      if (_movementPath.state == MovementPath.State.OneWay && _movementPath.ReachedEnd)
      {
        return typeof(AIIdleState);
      }

      return null;
    }

    public override void OnEnter()
    {
      return;
    }
    public override void OnExit()
    {
      return;
    }
  }
}