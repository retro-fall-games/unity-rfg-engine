using System.Collections;
using UnityEngine;
using RFG.Input;

namespace RFG.Platformer
{
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
      _character.CharacterInput.PauseButton.State.OnStateChange += JumpButtonOnStateChanged;
    }

    public override void ProcessBehavior()
    {

    }

    private void JumpButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:

          break;
        case ButtonStates.Up:

          break;
      }
    }


  }
}