using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class Transitions : Singleton<Transitions>
  {
    [Header("Settings")]
    public bool ShowOnStart = false;
    [Range(0, 1)]
    public float Speed = 1f;

    [Header("Transitions")]
    public GameObject[] transitions;

    [Header("Starting Transition")]
    public string StartingTransition;

    [HideInInspector]
    private Dictionary<string, Animator> _transitions = new Dictionary<string, Animator>();

    protected override void Awake()
    {
      base.Awake();
      foreach (GameObject transition in transitions)
      {
        Animator animator = transition.GetComponent<Animator>();
        if (animator)
        {
          animator.speed = Speed;
          _transitions.Add(transition.name, animator);
        }
      }
      if (!StartingTransition.Equals(""))
      {
        Constant(StartingTransition);
      }
    }

    private void Start()
    {
      if (ShowOnStart)
      {
        End(StartingTransition);
      }
    }

    public void Start(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        transition.Play("Start");
      }
    }

    public void End(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        transition.Play("End");
      }
    }

    public void Constant(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        transition.Play("Constant");
      }
    }

    public string GetCurrentClip(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        AnimatorClipInfo[] currentClipInfo = transition.GetCurrentAnimatorClipInfo(0);
        if (currentClipInfo.Length > 0)
        {
          return currentClipInfo[0].clip.name;
        }
      }
      return null;
    }

  }
}