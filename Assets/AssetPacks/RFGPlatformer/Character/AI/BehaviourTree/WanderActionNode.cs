using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.BehaviourTree;

namespace RFG
{
  namespace Platformer
  {
    public class WanderActionNode : ActionNode
    {
      protected override void OnStart()
      {
      }

      protected override void OnStop()
      {
        AIBrainBehaviour brain = context as AIBrainBehaviour;
        brain.Context.MoveHorizontally(0);
      }

      protected override State OnUpdate()
      {
        AIBrainBehaviour brain = context as AIBrainBehaviour;
        if (brain.Context.characterContext.settingsPack == null || brain.Context.characterContext.settingsPack.WalkingSettings == null)
          return State.Success;

        WalkingSettings WalkingSettings = brain.Context.characterContext.settingsPack.WalkingSettings;
        brain.Context.FlipOnCollision();
        brain.Context.FlipOnDangle();
        if (!brain.Context.JustRotated())
        {
          brain.Context.FlipOnLevelBoundsCollision();
        }
        brain.Context.controller.State.IsWalking = true;
        brain.Context.MoveHorizontally(WalkingSettings.WalkingSpeed);
        brain.Context.TouchingWalls();
        return State.Running;
      }
    }
  }
}