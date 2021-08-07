using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Death Character State", menuName = "RFG/Platformer/Character/Character State/Death")]
    public class DeathCharacterState : CharacterState
    {
      public override Type Execute(Character character)
      {
        // If the character type is player we want to execute the death state and 
        // then respawn
        if (character.CharacterType == CharacterType.Player)
        {
          return typeof(SpawnCharacterState);
        }
        return null;
      }

      public override void Exit(Character character)
      {
        base.Exit(character);
        character.Controller.enabled = false;

        // If the character type is not a player then we want to remove that character
        // when the death state exits
        if (character.CharacterType != CharacterType.Player)
        {
          if (character.ObjectPool)
          {
            character.gameObject.SetActive(false);
          }
          else
          {
            Destroy(character.gameObject);
          }
        }
      }
    }
  }
}