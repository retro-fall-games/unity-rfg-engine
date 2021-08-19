using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Double Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Double Jump")]
    public class DoubleJumpAbility : CharacterAbility
    {
      public override void Init(CharacterAbilityController.AbilityContext ctx)
      {
        JumpAbility ability = ctx.controller.FindAbility<JumpAbility>();
        ability.NumberOfJumps = 2;
        ability.SetNumberOfJumpsLeft(ability.NumberOfJumps);
      }

      public override void Remove(CharacterAbilityController.AbilityContext ctx)
      {
        JumpAbility ability = ctx.controller.FindAbility<JumpAbility>();
        ability.NumberOfJumps = 1;
        ability.SetNumberOfJumpsLeft(ability.NumberOfJumps);
      }
    }
  }
}