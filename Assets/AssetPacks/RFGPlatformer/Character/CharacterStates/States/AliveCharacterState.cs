using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Alive Character State", menuName = "RFG/Platformer/Character/Character State/Alive")]
    public class AliveCharacterState : CharacterState
    {
      public override Type Execute(Character character)
      {
        character.Abilities.Process();
        return null;
      }
    }
  }
}