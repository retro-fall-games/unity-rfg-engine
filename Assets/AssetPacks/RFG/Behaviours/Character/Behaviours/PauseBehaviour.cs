using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Character/Behaviour/Pause Behaviour")]
  public class PauseBehaviour : BaseCharacterBehaviour
  {
    public override void InitBehaviour()
    {
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => InputManager.Instance != null);
      yield return new WaitUntil(() => InputManager.Instance.PauseButton != null);
      InputManager.Instance.PauseButton.State.OnStateChange += PauseButtonOnStateChanged;
    }

    private void PauseButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          // EventManager.TriggerEvent(new GameEvent(GameEvent.GameEventType.Pause));
          break;
        case ButtonStates.Up:

          break;
      }
    }

  }
}