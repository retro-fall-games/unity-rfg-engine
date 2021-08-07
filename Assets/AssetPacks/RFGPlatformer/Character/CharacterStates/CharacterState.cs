using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public abstract class CharacterState : ScriptableObject
    {
      [Header("Sound FX")]
      public SoundData[] EnterFx;
      public SoundData[] ExitFx;

      public virtual void Enter(Character character)
      {
        if (EnterFx.Length > 0)
        {
          SoundManager.Instance.Play(EnterFx);
        }
      }
      public virtual Type Execute(Character character)
      {
        return null;
      }
      public virtual void Exit(Character character)
      {
        if (ExitFx.Length > 0)
        {
          SoundManager.Instance.Play(ExitFx);
        }
      }
    }
  }
}