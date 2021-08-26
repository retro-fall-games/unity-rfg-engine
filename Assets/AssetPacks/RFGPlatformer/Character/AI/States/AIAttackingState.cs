using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Attacking State", menuName = "RFG/Platformer/Character/AI State/Attacking")]
    public class AIAttackingState : AIState
    {
      public override Type Execute(AIBrainBehaviour.AIStateContext ctx)
      {
        // Rotate to always face the target
        ctx.controller.RotateTowards(ctx.aggro.target2);

        // Move towards that target
        float normalizedHorizontalSpeed = 0f;

        if (ctx.controller.State.IsFacingRight)
        {
          normalizedHorizontalSpeed = 1f;
        }
        else
        {
          normalizedHorizontalSpeed = -1f;
        }

        ctx.transform.SpawnFromPool("Effects", ctx.aiBrain.RunningSettings.RunningEffects);
        ctx.animator.Play(ctx.aiBrain.RunningSettings.RunningClip);

        float movementFactor = ctx.controller.Parameters.GroundSpeedFactor;
        float movementSpeed = normalizedHorizontalSpeed * ctx.aiBrain.RunningSettings.RunningSpeed * ctx.controller.Parameters.SpeedFactor;
        float horizontalMovementForce = Mathf.Lerp(ctx.controller.Velocity.x, movementSpeed, Time.deltaTime * movementFactor);

        ctx.controller.SetHorizontalForce(horizontalMovementForce);

        //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
        //   {
        //     ctx.equipmentSet.PrimaryWeapon?.Perform();
        //   }

        //   if (ctx.character.CurrentStateType == typeof(AIAttackingState))
        //   {
        //     ctx.equipmentSet.SecondaryWeapon?.Perform();
        //   }

        return null;
      }
    }
  }
}