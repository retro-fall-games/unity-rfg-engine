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

      public override void Consume(Inventory inventory)
      {
        Character character = inventory.GetComponent<Character>();
        Debug.Log("You used max health: " + MaxHealthToAdd);
        // Find Health
        // Add Health
      }
    }
  }
}