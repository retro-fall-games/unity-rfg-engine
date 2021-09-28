using UnityEngine;

namespace RFG.Platformer
{
  using Items;

  [CreateAssetMenu(fileName = "New Max Health Item", menuName = "RFG/Platformer/Items/Consumable/Max Health")]
  public class MaxHealthItem : Consumable
  {
    [Header("Max Health Item Settings")]
    public int MaxHealthToAdd = 1;

    public override void Consume(Inventory inventory, bool showEffects = true)
    {
      base.Consume(inventory, showEffects);
      HealthBehaviour health = inventory.GetComponent<HealthBehaviour>();
      if (health != null)
      {
        health.AddMaxHealth(MaxHealthToAdd);
      }
    }
  }
}