using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public abstract class CharacterState : ScriptableObject
    {
      [Header("Animation")]
      public string Layer = "Base Layer";
      public string EnterClip;
      public string ExitClip;

      [Header("Sound FX")]
      public SoundData[] EnterFx;
      public SoundData[] ExitFx;

      protected Character _character;
      protected Animator _animator;
      private int _layerIndex;

      public virtual void Init(Character character)
      {
        _character = character;
        _animator = character.GetComponent<Animator>();
        _layerIndex = _animator.GetLayerIndex(Layer);
      }

      public virtual void Enter()
      {
        if (EnterFx.Length > 0)
        {
          SoundManager.Instance.Play(EnterFx);
        }
        int hash = Animator.StringToHash(EnterClip);
        if (_animator.HasState(_layerIndex, hash))
        {
          _animator.Play(EnterClip);
        }
      }

      public virtual Type Execute()
      {
        return null;
      }

      public virtual void Exit()
      {
        if (ExitFx.Length > 0)
        {
          SoundManager.Instance.Play(ExitFx);
        }
        int hash = Animator.StringToHash(ExitClip);
        if (_animator.HasState(_layerIndex, hash))
        {
          _animator.Play(EnterClip);
        }
      }
    }
  }
}