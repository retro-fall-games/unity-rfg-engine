using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Max Health Item", menuName = "RFG/Platformer/Items/Consumable/Max Health")]
    public class MaxHealthItem : Consumable
    {
      [Header("Max Health Item Settings")]
      public int MaxHealthToAdd = 1;

      public override void Consume(Inventory inventory, bool showEffects = true)
      {
        base.Consume(inventory, showEffects);
        CharacterBehaviourController behaviour = inventory.GetComponent<CharacterBehaviourController>();
        if (behaviour != null)
        {
          HealthBehaviour health = behaviour.FindBehavior<HealthBehaviour>();
          if (health != null)
          {
            health.AddMaxHealth(MaxHealthToAdd);
          }
        }
      }
    }
  }
}