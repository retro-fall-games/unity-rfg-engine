using UnityEngine;
using RFG.Input;
using RFG.Events;
using RFG.Core;

namespace RFG.Platformer
{
  public class CharacterInput : MonoBehaviour
  {
    public Button JumpButton { get; private set; }
    public Button PauseButton { get; private set; }
    public InputManager InputManager => _inputManager;
    private InputManager _inputManager;

    private void Awake()
    {
      _inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;
    }

    private void Start()
    {
      _inputManager.AddButton(JumpButton = new Button("Jump"));
      _inputManager.AddButton(PauseButton = new Button("Pause"));

      PauseButton.State.OnStateChange += PauseMenuButtonOnStateChange;
    }

    private void PauseMenuButtonOnStateChange(ButtonStates state)
    {
      if (state == ButtonStates.Down)
      {
        EventManager.TriggerEvent(new GameEvent(GameEvent.GameEventType.Pause));
      }
    }
  }
}
