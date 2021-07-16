using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behavior/Pause Behavior")]
  public class PauseBehavior : CharacterBehavior
  {

    public override void InitBehavior()
    {
      StartCoroutine(InitBehaviorCo());
    }

    private IEnumerator InitBehaviorCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput.InputManager != null);
      yield return new WaitUntil(() => _character.CharacterInput.PauseButton != null);
      _character.CharacterInput.PauseButton.State.OnStateChange += PauseButtonOnStateChanged;
    }

    public override void ProcessBehavior()
    {

    }

    private void PauseButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          EventManager.TriggerEvent(new GameEvent(GameEvent.GameEventType.Pause));
          break;
        case ButtonStates.Up:

          break;
      }
    }


  }
}