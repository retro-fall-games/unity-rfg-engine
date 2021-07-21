using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Core/Managers/Input Manager")]
  public class InputManager : Singleton<InputManager>
  {
    [Header("Settings")]
    public string axisHortizontal = "Horizontal";
    public string axisVertical = "Vertical";
    public Vector2 threshold = new Vector2(0.1f, 0.4f);

    [Header("Cursor")]
    public Texture2D customCursor;

    [Header("Joystick Pack")]
    public VariableJoystick variableJoystick;

    public Vector2 PrimaryMovement => _primaryMovement;
    private Dictionary<string, Button> _buttons;
    private Vector2 _primaryMovement = Vector2.zero;

    private void Start()
    {
      _buttons = new Dictionary<string, Button>();
      if (customCursor != null)
      {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.ForceSoftware);
      }
    }

    private void Update()
    {
      // Set the primary movement
      if (variableJoystick != null)
      {
        _primaryMovement = variableJoystick.Direction;
      }
      else
      {
        _primaryMovement.x = UnityEngine.Input.GetAxis(axisHortizontal);
        _primaryMovement.y = UnityEngine.Input.GetAxis(axisVertical);
      }

      // Check all button states
      foreach (KeyValuePair<string, Button> item in _buttons)
      {
        Button button = item.Value;
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
      foreach (KeyValuePair<string, Button> item in _buttons)
      {
        Button button = item.Value;
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
      _buttons.Add(button.buttonId, button);
    }

    public Button GetButton(string buttonId)
    {
      if (_buttons.ContainsKey(buttonId))
      {
        return _buttons[buttonId];
      }
      return null;
    }

  }
}
