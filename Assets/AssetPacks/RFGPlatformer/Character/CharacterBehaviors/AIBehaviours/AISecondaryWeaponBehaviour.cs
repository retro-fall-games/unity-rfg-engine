using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New AI Secondary Weapon Character Behaviour", menuName = "RFG/Platformer/Character/Character AI Behaviour/Secondary Weapon")]
    public class AISecondaryWeaponBehaviour : CharacterBehaviour
    {
      public override void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
        if (ctx.character.AIState.CurrentStateType == typeof(AIAttackingState))
        {
          ctx.equipmentSet.SecondaryWeapon?.Perform();
        }
      }
    }
  }
}