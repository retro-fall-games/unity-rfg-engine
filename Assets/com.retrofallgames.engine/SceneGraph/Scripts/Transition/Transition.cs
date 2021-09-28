using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.SceneGraph
{
  [AddComponentMenu("RFG/Scene/Transition")]
  public class Transition : MonoBehaviour
  {
    [Header("Settings")]
    [Range(0, 1)]
    public float Speed = 1f;

    [Header("Starting Transition")]
    public bool ShowOnStart = false;
    public string StartingOnTransition;
    public float WaitForSeconds = 1f;

    [HideInInspector]
    private Dictionary<string, Animator> _transitions = new Dictionary<string, Animator>();

    private void Awake()
    {
      foreach (Transform child in transform)
      {
        Animator animator = child.GetComponent<Animator>();
        if (animator)
        {
          animator.speed = Speed;
          _transitions.Add(child.name, animator);
        }
      }
    }

    private void Start()
    {
    }

    private IEnumerator StartCo()
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      End(StartingOnTransition);
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

    public void On(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        transition.Play("On");
      }
    }

    public void Off(string name)
    {
      if (_transitions.ContainsKey(name))
      {
        Animator transition = _transitions[name];
        transition.Play("Off");
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

    private void OnEnable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
      if (!StartingOnTransition.Equals(""))
      {
        On(StartingOnTransition);
      }
      if (ShowOnStart)
      {
        StartCoroutine(StartCo());
      }
    }

  }
}