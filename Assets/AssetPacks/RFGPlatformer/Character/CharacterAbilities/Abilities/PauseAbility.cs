using UnityEngine;
using UnityEngine.InputSystem;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Pause Character Ability", menuName = "RFG/Platformer/Character/Character Ability/Pause")]
    public class PauseAbility : CharacterAbility
    {
      [Header("Game Events")]
      public GameEvent PauseEvent;

      [Header("Effects")]
      public string[] PauseEffects;
      public string[] UnPauseEffects;

      public override void OnButtonPerformed(InputAction.CallbackContext inputCtx, CharacterAbilityController.AbilityContext ctx)
      {
        if (GameManager.Instance.IsPaused)
        {
          ctx.transform.SpawnFromPool("Effects", PauseEffects);
        }
        else
        {
          ctx.transform.SpawnFromPool("Effects", UnPauseEffects);
        }
        PauseEvent?.Raise();
      }

    }
  }
}