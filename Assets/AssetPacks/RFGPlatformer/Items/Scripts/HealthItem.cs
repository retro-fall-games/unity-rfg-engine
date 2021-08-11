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

      public override void Use(Inventory inventory)
      {
        Character character = inventory.GetComponent<Character>();
        Debug.Log("You used health: " + HealthToAdd);
        // Find Health
        // Add Health
      }
    }
  }
}