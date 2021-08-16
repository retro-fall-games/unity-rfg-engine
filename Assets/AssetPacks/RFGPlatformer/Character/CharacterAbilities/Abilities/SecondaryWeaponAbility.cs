using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Secondary Weapon Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Secondary Weapon")]
    public class SecondaryWeaponAbility : CharacterAbility
    {
      private Weapon _weapon;

      public override void Init(CharacterAbilityController.AbilityContext ctx)
      {
        EquipmentSet equipmentSet = ctx.character.GetComponent<EquipmentSet>();
        equipmentSet.OnEquipSecondaryWeapon += OnEquipSecondaryWeapon;
      }

      public override void Remove(CharacterAbilityController.AbilityContext ctx)
      {
        EquipmentSet equipmentSet = ctx.character.GetComponent<EquipmentSet>();
        equipmentSet.OnEquipSecondaryWeapon -= OnEquipSecondaryWeapon;
      }

      private void OnEquipSecondaryWeapon(Weapon weapon)
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