using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Character Dead State", menuName = "RFG/Platformer/Character/Character State/Dead")]
    public class DeadState : CharacterState
    {
      public override Type Execute(CharacterStateController.CharacterStateContext ctx)
      {
        return null;
      }
    }
  }
}