using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Spawn State", menuName = "RFG/Platformer/Character/State/Spawn")]
    public class SpawnState : State
    {
      public override void Enter(Transform transform, Animator animator)
      {
        base.Enter(transform, animator);

        HealthBehaviour health = transform.GetComponent<HealthBehaviour>();
        if (health != null)
        {
          health.ResetHealth();
        }
        Character character = transform.GetComponent<Character>();
        if (character.CharacterType == CharacterType.Player)
        {
          character.CalculatePlayerSpawnAt();
        }
        transform.gameObject.SetActive(true);
        character.Controller.ResetVelocity();
        character.Controller.enabled = true;
        character.EnableAllAbilities();
      }

      public override Type Execute(Transform transform, Animator animator)
      {
        Character character = transform.GetComponent<Character>();
        if (character.SpawnAt != null)
        {
          transform.position = character.SpawnAt.position;
        }
        return typeof(AliveState);
      }

    }
  }
}