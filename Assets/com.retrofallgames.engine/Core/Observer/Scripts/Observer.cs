using System;
using UnityEngine;

namespace RFG.Core
{
  public class Observer<T> : ScriptableObject
  {
    public event Action<T> OnRaise;

    public void Raise(T param)
    {
      OnRaise?.Invoke(param);
    }
  }
}