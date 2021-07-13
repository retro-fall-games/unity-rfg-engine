using System.Collections.Generic;
using UnityEngine;
using RFG.Utils;
using RFG.Input;
using RFG.Events;

namespace RFG.Core
{
  [AddComponentMenu("RFG Engine/Core/Managers/Input Manager")]
  public class InputManager : Singleton<InputManager>
  {
    public string axisHortizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public Vector2 threshold = new Vector2(0.1f, 0.4f);
    public Button JumpButton { get; private set; }
    public Button PauseMenuButton { get; private set; }
    public Vector2 PrimaryMovement => _primaryMovement;
    private List<Button> _buttons;
    private Vector2 _primaryMovement = Vector2.zero;

    private void Start()
    {
      _buttons = new List<Button>();
    }

    private void Update()
    {
      // Set the primary movement
      _primaryMovement.x = UnityEngine.Input.GetAxis(axisHortizontal);
      _primaryMovement.y = UnityEngine.Input.GetAxis(axisVertical);

      // Check all button states
      foreach (Button button in _buttons)
      {
        if (UnityEngine.Input.GetButton(button.buttonId))
        {
          button.State.ChangeState(ButtonStates.Pressed);
        }
        if (UnityEngine.Input.GetButtonDown(button.buttonId))
        {
          button.State.ChangeState(ButtonStates.Down);
        }
        if (UnityEngine.Input.GetButtonUp(button.buttonId))
        {
          button.State.ChangeState(ButtonStates.Up);
        }
      }
    }

    private void LateUpdate()
    {
      // After all Updates have run, change button states
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

    public void AddButton(Button button)
    {
      _buttons.Add(button);
    }
  }
}
