using UnityEngine;
using RFG.Utils;

namespace RFG.Input
{
  public enum ButtonStates { Off, Down, Pressed, Up }

  public class Button
  {
    public StateMachine<ButtonStates> State { get; private set; }
    public string buttonId;
    public float TimeSinceLastButtonDown { get { return Time.unscaledTime - _lastButtonDownAt; } }
    public float TimeSinceLastButtonUp { get { return Time.unscaledTime - _lastButtonUpAt; } }
    private float _lastButtonDownAt;
    private float _lastButtonUpAt;

    public Button(string buttonId)
    {
      this.buttonId = buttonId;
      State = new StateMachine<ButtonStates>(null, false);
      State.ChangeState(ButtonStates.Off);
      State.OnStateChange += OnStateChange;
    }

    public bool ButtonDownRecently(float time)
    {
      return (Time.unscaledTime - TimeSinceLastButtonDown <= time);
    }
    public bool ButtonUpRecently(float time)
    {
      return (Time.unscaledTime - TimeSinceLastButtonUp <= time);
    }

    private void OnStateChange(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          _lastButtonDownAt = Time.unscaledTime;
          break;
        case ButtonStates.Up:
          _lastButtonUpAt = Time.unscaledTime;
          break;
      }
    }

  }
}