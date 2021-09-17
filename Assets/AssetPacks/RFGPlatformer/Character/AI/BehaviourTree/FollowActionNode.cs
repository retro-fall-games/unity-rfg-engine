using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.BehaviourTree;

namespace RFG
{
  namespace Platformer
  {
    public class FollowActionNode : ActionNode
    {
      protected override void OnStart()
      {
      }

      protected override void OnStop()
      {
      }

      protected override State OnUpdate()
      {
        AIBrainBehaviour brain = context as AIBrainBehaviour;
        if (brain.Context.JustRotated())
        {
          return State.Running;
        }
        if (brain.Context.RotateTowards())
        {
          return State.Running;
        }
        if (brain.Context.PauseOnDangle())
        {
          return State.Running;
        }
        float speed = brain.Context.WalkOrRun();
        brain.Context.MoveHorizontally(speed);
        brain.Context.MoveVertically(speed);
        return State.Running;
      }
    }
  }
}