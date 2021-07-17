using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/Pause Behaviour")]
  public class PauseBehaviour : CharacterBehaviour
  {

    public override void InitBehaviour()
    {
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput.InputManager != null);
      yield return new WaitUntil(() => _character.CharacterInput.PauseButton != null);
      _character.CharacterInput.PauseButton.State.OnStateChange += PauseButtonOnStateChanged;
    }

    public override void ProcessBehaviour()
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