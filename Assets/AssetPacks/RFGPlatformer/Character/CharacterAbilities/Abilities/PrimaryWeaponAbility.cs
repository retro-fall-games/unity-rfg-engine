using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Primary Weapon Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Primary Weapon")]
    public class PrimaryWeaponAbility : CharacterAbility
    {

      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.PrimaryWeapon?.Started();
        }
      }

      public override void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.PrimaryWeapon?.Cancel();
        }
      }

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.PrimaryWeapon?.Perform();
        }
      }

    }

  }
}