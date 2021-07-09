using System.Collections.Generic;
using UnityEngine;
using RFG.Utils.Singletons;
using RFG.Engine.Input;
using RFG.Events;

namespace RFG.Engine.Managers
{
  [AddComponentMenu("RFG Engine/Managers/Input Manager")]
  public class InputManager : Singleton<InputManager>
  {
    public Button JumpButton { get; private set; }
    public Button PauseMenuButton { get; private set; }
    private List<Button> _buttons;

    private void Start()
    {
      InitializeButtons();
    }

    private void InitializeButtons()
    {
      _buttons = new List<Button>();
      _buttons.Add(JumpButton = new Button("Jump"));
      _buttons.Add(PauseMenuButton = new Button("PauseMenu"));

      PauseMenuButton.State.OnStateChange += PauseMenuButtonOnStateChange;
    }

    private void LateUpdate()
    {
      ProcessButtonStates();
    }

    private void Update()
    {
      GetInputButtons();
    }

    private void GetInputButtons()
    {
      foreach (Button button in _buttons)
      {
        if (UnityEngine.Input.GetButton(button.buttonId))
        {
          button.TriggerButtonPressed();
        }
        if (UnityEngine.Input.GetButtonDown(button.buttonId))
        {
          button.TriggerButtonDown();
        }
        if (UnityEngine.Input.GetButtonUp(button.buttonId))
        {
          button.TriggerButtonUp();
        }
      }
    }

    private void ProcessButtonStates()
    {
      foreach (Button button in _buttons)
      {
        if (button.State.CurrentState == ButtonStates.Down)
        {
          button.State.ChangeState(ButtonStates.Pressed);
        }
        if (button.State.CurrentState == ButtonStates.Up)
        {
          button.State.ChangeState(ButtonStates.Off);
        }
      }
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
