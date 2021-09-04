using System;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class AIState : ScriptableObject
    {
      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to run when the state enters")]
      public string EnterClip;

      [Tooltip("Define what animation to run when the state exits")]
      public string ExitClip;

      [Header("Effects")]
      [Tooltip("Define what effects to run when the state exits")]
      public string[] EnterEffects;
      [Tooltip("Define what effect to run when the state exits")]
      public string[] ExitEffects;

      public virtual void Enter(AIStateContext ctx)
      {
        PlayEffects(ctx.transform, EnterEffects);
        PlayAnimations(ctx.animator, EnterClip);
      }

      public virtual Type Execute(AIStateContext ctx)
      {
        return null;
      }

      public virtual void Exit(AIStateContext ctx)
      {
        PlayEffects(ctx.transform, ExitEffects);
        PlayAnimations(ctx.animator, ExitClip);
      }

      protected void PlayAnimations(Animator animator, string clip)
      {
        int hash = Animator.StringToHash(clip);
        int layerIndex = animator.GetLayerIndex(Layer);
        if (animator.HasState(layerIndex, hash))
        {
          animator.Play(clip);
        }
      }

      protected void PlayEffects(Transform transform, string[] effects)
      {
        transform.SpawnFromPool("Effects", effects);
      }
    }
  }
}