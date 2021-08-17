using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Primary Weapon Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Primary Weapon")]
    public class AIPrimaryWeaponBehaviour : CharacterBehaviour
    {
      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType == typeof(AIAttackingState))
        {
          ctx.equipmentSet.PrimaryWeapon?.Perform();
        }
      }
    }
  }
}