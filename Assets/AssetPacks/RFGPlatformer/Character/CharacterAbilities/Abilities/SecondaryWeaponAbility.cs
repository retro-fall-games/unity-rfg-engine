using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Secondary Weapon Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Secondary Weapon")]
    public class SecondaryWeaponAbility : CharacterAbility
    {
      public override void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.SecondaryWeapon?.Started();
        }
      }

      public override void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.SecondaryWeapon?.Cancel();
        }
      }

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        bool pointerOverUi = EventSystem.current.IsPointerOverGameObject();
        if (!pointerOverUi)
        {
          ctx.equipmentSet.SecondaryWeapon?.Perform();
        }
      }

    }

  }
}