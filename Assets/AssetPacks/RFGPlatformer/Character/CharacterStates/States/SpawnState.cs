using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Spawn State", menuName = "RFG/Platformer/Character/Character State/Spawn")]
    public class SpawnState : CharacterState
    {
      public override void Enter()
      {
        base.Enter();

        // HealthBehaviour health = FindBehaviour<HealthBehaviour>();
        // if (health != null)
        // {
        //   health.Reset();
        // }

        _character.gameObject.SetActive(true);
        _character.Controller.ResetVelocity();
        _character.Controller.enabled = true;
      }
      public override Type Execute()
      {
        _character.transform.position = _character.SpawnAt.position;
        return typeof(AliveState);
      }

    }
  }
}