using System.Collections.Generic;
using UnityEngine;
using RFG.Utils;
using RFG.Input;
using RFG.Events;

namespace RFG.Managers
{
  [AddComponentMenu("RFG Engine/Managers/Input Manager")]
  public class InputManager : Singleton<InputManager>
  {
    public string axisHortizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public Button JumpButton { get; private set; }
    public Button PauseMenuButton { get; private set; }
    public Vector2 PrimaryMovement => _primaryMovement;
    private List<Button> _buttons;
    private Vector2 _primaryMovement = Vector2.zero;

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

    private void Update()
    {
      SetMovement();
      GetInputButtons();
    }

    private void LateUpdate()
    {
      ProcessButtonStates();
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

    private void SetMovement()
    {
      _primaryMovement.x = UnityEngine.Input.GetAxis(axisHortizontal);
      _primaryMovement.y = UnityEngine.Input.GetAxis(axisVertical);
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
