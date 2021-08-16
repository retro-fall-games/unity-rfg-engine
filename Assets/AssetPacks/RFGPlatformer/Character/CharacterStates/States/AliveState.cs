using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Character Alive State", menuName = "RFG/Platformer/Character/Character State/Alive")]
    public class AliveState : CharacterState
    {
      public override Type Execute(CharacterStateController.CharacterStateContext ctx)
      {
        ctx.character.Abilities?.Process();
        return null;
      }
    }
  }
}