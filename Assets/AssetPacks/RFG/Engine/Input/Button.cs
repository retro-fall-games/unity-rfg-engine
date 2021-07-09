using UnityEngine;
using RFG.Utils.StateMachines;

namespace RFG.Engine.Input
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
      State = new StateMachine<ButtonStates>(null);
      State.ChangeState(ButtonStates.Off);
    }

    public bool ButtonDownRecently(float time)
    {
      return (Time.unscaledTime - TimeSinceLastButtonDown <= time);
    }
    public bool ButtonUpRecently(float time)
    {
      return (Time.unscaledTime - TimeSinceLastButtonUp <= time);
    }
    public void TriggerButtonDown()
    {
      _lastButtonDownAt = Time.unscaledTime;
      State.ChangeState(ButtonStates.Down);
    }

    public void TriggerButtonPressed()
    {
      State.ChangeState(ButtonStates.Pressed);
    }

    public void TriggerButtonUp()
    {
      _lastButtonUpAt = Time.unscaledTime;
      State.ChangeState(ButtonStates.Up);
    }

  }
}