using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Primary Weapon Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Primary Weapon")]
    public class PrimaryWeaponAbility : CharacterAbility
    {
      private Weapon _weapon;

      public override void Init(CharacterAbilityController.AbilityContext ctx)
      {
        EquipmentSet equipmentSet = ctx.character.GetComponent<EquipmentSet>();
        equipmentSet.OnEquipPrimaryWeapon += OnEquipPrimaryWeapon;
      }

      public override void Remove(CharacterAbilityController.AbilityContext ctx)
      {
        EquipmentSet equipmentSet = ctx.character.GetComponent<EquipmentSet>();
        equipmentSet.OnEquipPrimaryWeapon -= OnEquipPrimaryWeapon;
      }

      private void OnEquipPrimaryWeapon(Weapon weapon)
      {
        _weapon = weapon;
      }

      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        _weapon?.Started();
      }

      public override void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        _weapon?.Cancel();
      }

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        _weapon?.Perform();
      }

    }

  }
}