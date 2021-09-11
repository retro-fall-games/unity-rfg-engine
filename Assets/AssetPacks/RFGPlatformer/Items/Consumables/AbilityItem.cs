using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Ability Item", menuName = "RFG/Platformer/Items/Consumable/Ability")]
    public class AbilityItem : Consumable
    {
      public enum AbilityType { DoubleJump, Dash, WallJump };

      [Header("Ability Settings")]
      public AbilityType AbilityToAdd = AbilityType.DoubleJump;

      public override void Consume(Inventory inventory, bool showEffects = true)
      {
        base.Consume(inventory, showEffects);

        switch (AbilityToAdd)
        {
          case AbilityType.DoubleJump:
            Character character = inventory.GetComponent<Character>();
            if (character != null)
            {
              character.Context.settingsPack.JumpSettings.NumberOfJumps = 2;
            }
            break;
          case AbilityType.Dash:
            DashAbility dashAbility = inventory.GetComponent<DashAbility>();
            if (dashAbility != null)
            {
              dashAbility.HasAbility = true;
            }
            break;
          case AbilityType.WallJump:
            WallClingingAbility wallClingingAbility = inventory.GetComponent<WallClingingAbility>();
            if (wallClingingAbility != null)
            {
              wallClingingAbility.HasAbility = true;
            }
            WallJumpAbility wallJumpAbility = inventory.GetComponent<WallJumpAbility>();
            if (wallJumpAbility != null)
            {
              wallJumpAbility.HasAbility = true;
            }
            break;
        }
      }
    }
  }
}