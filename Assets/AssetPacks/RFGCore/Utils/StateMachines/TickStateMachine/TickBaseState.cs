using System;
using UnityEngine;

namespace RFG
{
  public abstract class TickBaseState
  {

    protected GameObject gameObject;
    protected Transform transform;

    public TickBaseState(GameObject gameObject)
    {
      this.gameObject = gameObject;
      this.transform = gameObject.transform;
    }

    public abstract Type Tick();
    public abstract void OnEnter();
    public abstract void OnExit();
  }
}