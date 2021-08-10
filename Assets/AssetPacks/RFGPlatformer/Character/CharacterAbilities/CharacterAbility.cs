using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    public abstract class CharacterAbility : ScriptableObject
    {
      public enum InputMethod { Movement, Jump, PrimaryAttack, SecondaryAttack, Pause, Dash }
      public InputMethod Input;
      public abstract void Init(Character character);
      public abstract void EarlyProcess();
      public abstract void Process();
      public abstract void LateProcess();
      public abstract void OnButtonStarted(InputAction.CallbackContext ctx);
      public abstract void OnButtonCanceled(InputAction.CallbackContext ctx);
      public abstract void OnButtonPerformed(InputAction.CallbackContext ctx);
    }
  }
}