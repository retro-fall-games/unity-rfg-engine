using System;
using UnityEngine;
using UnityEngine.InputSystem;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [Serializable]
    public class CharacterAbilitySave
    {
      public string Guid;
    }

    public class CharacterAbility : ScriptableObject
    {
      [Header("Settings")]
      [ReadOnly] public string Guid;
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

      public CharacterAbilitySave GetSave()
      {
        CharacterAbilitySave save = new CharacterAbilitySave();
        save.Guid = Guid;
        return save;
      }

#if UNITY_EDITOR
      [ButtonMethod]
      protected void GenerateGuid()
      {
        if (Guid == null || Guid.Equals(""))
        {
          Guid = System.Guid.NewGuid().ToString();
        }
      }
#endif
    }
  }
}