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
      public virtual void Init(Character character)
      {
      }
      public virtual void EarlyProcess()
      {
      }
      public virtual void Process()
      {
      }
      public virtual void LateProcess()
      {
      }
      public virtual void OnButtonStarted(InputAction.CallbackContext ctx)
      {
      }
      public virtual void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }
      public virtual void OnButtonPerformed(InputAction.CallbackContext ctx)
      {
      }
    }
  }
}