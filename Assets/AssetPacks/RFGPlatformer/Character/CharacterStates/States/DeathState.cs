using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Death State", menuName = "RFG/Platformer/Character/Character State/Death")]
    public class DeathState : CharacterState
    {
      public override Type Execute()
      {
        // If the character type is player we want to execute the death state and 
        // then respawn
        if (_character.CharacterType == CharacterType.Player)
        {
          return typeof(SpawnState);
        }
        return null;
      }

      public override void Exit()
      {
        base.Exit();
        _character.Controller.enabled = false;

        // If the character type is not a player then we want to remove that character
        // when the death state exits
        if (_character.CharacterType != CharacterType.Player)
        {
          if (_character.ObjectPool)
          {
            _character.gameObject.SetActive(false);
          }
          else
          {
            Destroy(_character.gameObject);
          }
        }
      }
    }
  }
}