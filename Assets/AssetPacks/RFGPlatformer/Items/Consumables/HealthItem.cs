using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Health Item", menuName = "RFG/Platformer/Items/Consumable/Health")]
    public class HealthItem : Consumable
    {
      [Header("Health Item Settings")]
      public int HealthToAdd = 5;

      public override void Consume(Inventory inventory)
      {
        CharacterBehaviourController behaviour = inventory.GetComponent<CharacterBehaviourController>();
        if (behaviour != null)
        {
          HealthBehaviour health = behaviour.FindBehavior<HealthBehaviour>();
          if (health != null)
          {
            health.AddHealth(HealthToAdd);
          }
        }
      }
    }
  }
}