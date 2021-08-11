using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Double Jump Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Double Jump")]
    public class DoubleJumpAbility : CharacterAbility
    {
      public override void Init(Character character)
      {
        CharacterAbilityController controller = character.GetComponent<CharacterAbilityController>();
        JumpAbility ability = controller.FindAbility<JumpAbility>();
        ability.NumberOfJumps = 2;
      }
    }
  }
}