using UnityEngine;

namespace RFG.Platformer
{
  using Items;

  [CreateAssetMenu(fileName = "New Health Item", menuName = "RFG/Platformer/Items/Consumable/Health")]
  public class HealthItem : Consumable
  {
    [Header("Health Item Settings")]
    public int HealthToAdd = 5;

    public override void Consume(Inventory inventory, bool showEffects = true)
    {
      base.Consume(inventory, showEffects);
      HealthBehaviour health = inventory.GetComponent<HealthBehaviour>();
      if (health != null)
      {
        health.AddHealth(HealthToAdd);
      }
    }
  }
}