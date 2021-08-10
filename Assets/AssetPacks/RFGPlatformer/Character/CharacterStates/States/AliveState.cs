using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Alive State", menuName = "RFG/Platformer/Character/Character State/Alive")]
    public class AliveState : CharacterState
    {
      public override Type Execute()
      {
        _character.Abilities.Process();
        return null;
      }
    }
  }
}