using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Spawn Character State", menuName = "RFG/Platformer/Character/Character State/Spawn")]
    public class SpawnCharacterState : CharacterState
    {
      public override void Enter(Character character)
      {
        base.Enter(character);

        // HealthBehaviour health = FindBehaviour<HealthBehaviour>();
        // if (health != null)
        // {
        //   health.Reset();
        // }

        character.gameObject.SetActive(true);
        character.Controller.ResetVelocity();
        character.Controller.enabled = true;
      }
      public override Type Execute(Character character)
      {
        character.transform.position = character.SpawnAt.position;
        return typeof(AliveCharacterState);
      }

    }
  }
}