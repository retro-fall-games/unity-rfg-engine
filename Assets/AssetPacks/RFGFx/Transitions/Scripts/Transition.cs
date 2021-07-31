using System.Collections.Generic;
using UnityEngine;


namespace RFG
{
  public class Transition : MonoBehaviour
  {
    public static Transition Instance { get; private set; }
    private Dictionary<string, Animator> transitions = new Dictionary<string, Animator>();

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        // Add transition animators here with key value pairs
        GameObject crossFadeObj = transform.Find("CrossFade").gameObject;
        if (crossFadeObj != null)
        {
          Animator crossFade = crossFadeObj.GetComponent<Animator>();
          if (crossFade != null)
          {
            transitions.Add("CrossFade", crossFade);
            // Begin in a faded transition, let the game handle when to come out of the fade
            Show("CrossFade", "Constant");
          }
        }
      }
    }

    public void Show(string name, string animation)
    {
      if (transitions.ContainsKey(name))
      {
        Animator transition = transitions[name];
        transition.Play(animation);
      }
    }

    public string GetCurrentClip(string name)
    {
      if (transitions.ContainsKey(name))
      {
        Animator transition = transitions[name];
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