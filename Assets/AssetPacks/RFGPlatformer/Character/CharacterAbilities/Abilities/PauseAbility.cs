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

      [Header("Sound FX")]
      public SoundData[] PauseFx;
      public SoundData[] UnPauseFx;

      public override void Init(Character character)
      {
      }

      public override void EarlyProcess()
      {
      }

      public override void Process()
      {
      }

      public override void LateProcess()
      {
      }

      public override void OnButtonStarted(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonCanceled(InputAction.CallbackContext ctx)
      {
      }

      public override void OnButtonPerformed(InputAction.CallbackContext ctx)
      {

        if (GameManager.Instance.IsPaused)
        {
          if (UnPauseFx.Length > 0)
          {
            SoundManager.Instance.Play(UnPauseFx);
          }
        }
        else
        {
          if (PauseFx.Length > 0)
          {
            SoundManager.Instance.Play(PauseFx);
          }
        }
        PauseEvent?.Raise();
      }

    }
  }
}