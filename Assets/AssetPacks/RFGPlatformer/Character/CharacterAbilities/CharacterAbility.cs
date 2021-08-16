using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    public class CharacterAbility : ScriptableObject
    {
      public enum InputMethod { Movement, Jump, PrimaryAttack, SecondaryAttack, Pause, Dash }
      public InputMethod Input;
      public virtual void Init(CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void Remove(CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void EarlyProcess(CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void Process(CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void LateProcess(CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void OnButtonStarted(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void OnButtonCanceled(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }
      public virtual void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
      }
    }
  }
}