using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Character Death State", menuName = "RFG/Platformer/Character/Character State/Death")]
    public class DeathState : CharacterState
    {
      public override Type Execute(CharacterStateController.CharacterStateContext ctx)
      {
        return typeof(DeadState);
      }

      public override void Exit(CharacterStateController.CharacterStateContext ctx)
      {
        base.Exit(ctx);
        if (ctx.character.CharacterType == CharacterType.Player)
        {
          GameManager.Instance.StartCoroutine(ctx.character.Respawn());
        }
        ctx.character.Controller.enabled = false;
        ctx.character.gameObject.SetActive(false);
      }

    }
  }
}