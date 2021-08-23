using System;
using UnityEngine;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RFG
{
  public class State : ScriptableObject
  {
    [Header("Settings")]
    [ReadOnly] public string Guid;

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

    public virtual void Enter(Transform transform, Animator animator)
    {
      PlayEffects(transform, EnterEffects);
      PlayAnimations(animator, EnterClip);
    }

    public virtual Type Execute(Transform transform, Animator animator)
    {
      return null;
    }

    public virtual void Exit(Transform transform, Animator animator)
    {
      PlayEffects(transform, ExitEffects);
      PlayAnimations(animator, ExitClip);
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

#if UNITY_EDITOR
    [ButtonMethod]
    protected void GenerateGuid()
    {
      if (Guid == null || Guid.Equals(""))
      {
        Guid = System.Guid.NewGuid().ToString();
        EditorUtility.SetDirty(this);
      }
    }
#endif
  }
}