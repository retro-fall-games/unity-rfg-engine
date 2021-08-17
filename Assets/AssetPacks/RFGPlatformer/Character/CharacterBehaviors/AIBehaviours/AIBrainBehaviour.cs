using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Brain Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Brain")]
    public class AIBrainBehaviour : CharacterBehaviour
    {
      [Header("Settings")]
      public float DecisionSpeed = 3f;
      public int DecisionOffset = 10;
      private float _decisionTimeElapsed = 0f;

      public override void InitValues(CharacterBehaviour behaviour)
      {
        AIBrainBehaviour b = (AIBrainBehaviour)behaviour;
        DecisionSpeed = b.DecisionSpeed;
        DecisionOffset = b.DecisionOffset;
      }

      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        _decisionTimeElapsed += Time.deltaTime;
        if (_decisionTimeElapsed >= DecisionSpeed)
        {
          _decisionTimeElapsed = 0;
          MakeDecision(ctx);
        }
      }

      public void MakeDecision(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType == typeof(AIIdleState))
        {
          int idleDecision = DecisionTree(1000, 500, 100 + DecisionOffset);
          switch (idleDecision)
          {
            case -1:
              ctx.character.AIState.ChangeState(typeof(AIWanderingState));
              break;
            case 1:
              ctx.character.AIState.ChangeState(typeof(AIIdleState));
              break;
            case 0:
            default:
              ctx.character.AIState.ChangeState(typeof(AIIdleState));
              break;
          }
        }
        else if (ctx.character.AIState.CurrentStateType == typeof(AIWanderingState))
        {
          int wanderingDecision = DecisionTree(1000, 500, 100 + DecisionOffset);
          switch (wanderingDecision)
          {
            case -1:
              ctx.character.AIMovementState.ChangeState(typeof(AIWalkingLeftState));
              break;
            case 1:
              ctx.character.AIMovementState.ChangeState(typeof(AIWalkingRightState));
              break;
            case 0:
            default:
              ctx.character.AIMovementState.ChangeState(typeof(AIIdleState));
              break;
          }

          int jumpDecision = DecisionTree(1000, 500, 100 + DecisionOffset);
          switch (jumpDecision)
          {
            case -1:
              ctx.character.AIMovementState.ChangeState(typeof(AIJumpingLeftState));
              break;
            case 1:
              ctx.character.AIMovementState.ChangeState(typeof(AIJumpingRightState));
              break;
            case 0:
            default:
              ctx.character.AIMovementState.ChangeState(typeof(AIIdleState));
              break;
          }
        }
        else if (ctx.character.AIState.CurrentStateType == typeof(AIAttackingState))
        {
          // if (_weaponBehavior != null)
          // {
          //   int weaponDecision = DecisionTree(1000, 500, 100 + decisionOffset);
          //   WeaponItem weapon;

          //   switch (weaponDecision)
          //   {
          //     case -1:
          //       weapon = _weaponBehavior.PrimaryWeapon;
          //       break;
          //     case 1:
          //       weapon = _weaponBehavior.SecondaryWeapon;
          //       break;
          //     case 0:
          //     default:
          //       weapon = _weaponBehavior.PrimaryWeapon;
          //       break;
          //   }

          //   if (weapon != null && weapon.weaponFiringState == WeaponItem.WeaponFiringState.Off)
          //   {
          //     weapon.Fire();
          //   }
          // }
        }
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