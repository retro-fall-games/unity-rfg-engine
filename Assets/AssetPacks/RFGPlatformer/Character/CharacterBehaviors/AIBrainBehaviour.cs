using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Engine/Character/Behaviour/AI Brain Behaviour")]
    public class AIBrainBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float decisionSpeed = 3f;
      public int decisionOffset = 10;
      private float _decisionTimeElapsed = 0f;
      private AIWeaponBehaviour _weaponBehavior;

      public override void InitBehaviour()
      {
        // _weaponBehavior = _character.FindBehaviour<AIWeaponBehaviour>();
      }

      public override void ProcessBehaviour()
      {
        _decisionTimeElapsed += Time.deltaTime;
        if (_decisionTimeElapsed >= decisionSpeed)
        {
          _decisionTimeElapsed = 0;
          MakeDecision();
        }
      }

      public void MakeDecision()
      {
        // if (_character.AIState.CurrentState == AIStates.Wandering)
        // {
        //   int wanderingDecision = DecisionTree(1000, 500, 100 + decisionOffset);
        //   switch (wanderingDecision)
        //   {
        //     case -1:
        //       _character.AIMovementState.ChangeState(AIMovementStates.WalkingLeft);
        //       break;
        //     case 1:
        //       _character.AIMovementState.ChangeState(AIMovementStates.WalkingRight);
        //       break;
        //     case 0:
        //     default:
        //       _character.AIMovementState.ChangeState(AIMovementStates.Idle);
        //       break;
        //   }

        //   int jumpDecision = DecisionTree(1000, 500, 100 + decisionOffset);
        //   switch (jumpDecision)
        //   {
        //     case -1:
        //       _character.AIMovementState.ChangeState(AIMovementStates.JumpingLeft);
        //       break;
        //     case 1:
        //       _character.AIMovementState.ChangeState(AIMovementStates.JumpingRight);
        //       break;
        //     case 0:
        //     default:
        //       _character.AIMovementState.ChangeState(AIMovementStates.Idle);
        //       break;
        //   }
        // }
        // else if (_character.AIState.CurrentState == AIStates.Attacking)
        // {
        //   if (_weaponBehavior != null)
        //   {
        //     int weaponDecision = DecisionTree(1000, 500, 100 + decisionOffset);
        //     WeaponItem weapon;

        //     switch (weaponDecision)
        //     {
        //       case -1:
        //         weapon = _weaponBehavior.PrimaryWeapon;
        //         break;
        //       case 1:
        //         weapon = _weaponBehavior.SecondaryWeapon;
        //         break;
        //       case 0:
        //       default:
        //         weapon = _weaponBehavior.PrimaryWeapon;
        //         break;
        //     }

        //     if (weapon != null && weapon.weaponFiringState == WeaponItem.WeaponFiringState.Off)
        //     {
        //       weapon.Fire();
        //     }
        //   }
        // }
      }

      private int DecisionTree(int range, int weight, int offset = 0)
      {
        int rand = UnityEngine.Random.Range(0, range);
        if (rand < weight)
        {
          return -1;
        }
        else if (rand >= weight && rand <= weight + offset)
        {
          return 0;
        }
        return 1;
      }

    }
  }
}