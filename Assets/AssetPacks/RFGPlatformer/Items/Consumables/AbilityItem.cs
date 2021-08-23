using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Ability Item", menuName = "RFG/Platformer/Items/Consumable/Ability")]
    public class AbilityItem : Consumable
    {
      // [Header("Ability Settings")]
      // public CharacterAbility[] AbilitiesToAdd;

      // public override void Consume(Inventory inventory, bool showEffects = true)
      // {
      //   base.Consume(inventory, showEffects);
      //   CharacterAbilityController controller = inventory.GetComponent<CharacterAbilityController>();
      //   foreach (CharacterAbility ability in AbilitiesToAdd)
      //   {
      //     controller.AddAbility(ability);
      //   }
      // }
    }
  }
}