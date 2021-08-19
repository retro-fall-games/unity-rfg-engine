using System;
using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    public abstract class CharacterState : ScriptableObject
    {
      [Header("Settings")]
      [ReadOnly] public string Guid;

      [Header("Animations")]
      public string Layer = "Base Layer";
      public string EnterClip;
      public string ExitClip;

      [Header("Effects")]
      public string[] EnterEffects;
      public string[] ExitEffects;

      public virtual void Enter(CharacterStateController.CharacterStateContext ctx)
      {
        PlayEffects(ctx.transform, EnterEffects);
        PlayAnimations(ctx.animator, EnterClip);
      }

      public virtual Type Execute(CharacterStateController.CharacterStateContext ctx)
      {
        return null;
      }

      public virtual void Exit(CharacterStateController.CharacterStateContext ctx)
      {
        PlayEffects(ctx.transform, ExitEffects);
        PlayAnimations(ctx.animator, ExitClip);
      }

      private void PlayAnimations(Animator animator, string Clip)
      {
        int hash = Animator.StringToHash(Clip);
        int layerIndex = animator.GetLayerIndex(Layer);
        if (animator.HasState(layerIndex, hash))
        {
          animator.Play(Clip);
        }
      }

      private void PlayEffects(Transform transform, string[] Effects)
      {
        transform.SpawnFromPool("Effects", Effects);
      }

#if UNITY_EDITOR
      [ButtonMethod]
      protected void GenerateGuid()
      {
        if (Guid == null || Guid.Equals(""))
        {
          Guid = System.Guid.NewGuid().ToString();
        }
      }
#endif
    }
  }
}