using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Character Spawn State", menuName = "RFG/Platformer/Character/Character State/Spawn")]
    public class SpawnState : CharacterState
    {
      public override void Enter(CharacterStateController.CharacterStateContext ctx)
      {
        base.Enter(ctx);

        CharacterBehaviourController controller = ctx.character.gameObject.GetComponent<CharacterBehaviourController>();
        if (controller != null)
        {
          HealthBehaviour health = controller.FindBehavior<HealthBehaviour>();
          if (health != null)
          {
            health.Reset();
          }
        }
        ctx.character.CalculatePlayerSpawnAt();
        ctx.character.gameObject.SetActive(true);
        ctx.character.Controller.ResetVelocity();
        ctx.character.Controller.enabled = true;
      }
      public override Type Execute(CharacterStateController.CharacterStateContext ctx)
      {
        ctx.character.transform.position = ctx.character.SpawnAt.position;
        return typeof(AliveState);
      }

    }
  }
}