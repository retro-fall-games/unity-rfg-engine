using UnityEngine;
using UnityEngine.Events;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Triggers/Trigger")]
  [RequireComponent(typeof(BoxCollider2D))]
  public class Trigger : MonoBehaviour
  {
    public bool onlyOnce = false;
    public string[] tags;
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;
    private bool _triggered = false;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (!_triggered && CheckTags(col.gameObject) == true)
      {
        _triggered = true;
        OnTriggerEnter?.Invoke();
      }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
      if (!_triggered && CheckTags(col.gameObject) == true)
      {
        if (!onlyOnce)
        {
          _triggered = false;
        }
        OnTriggerExit?.Invoke();
      }
    }

    private bool CheckTags(GameObject obj)
    {
      for (int i = 0; i < tags.Length; i++)
      {
        if (obj.CompareTag(tags[i]))
        {
          return true;
        }
      }
      return false;
    }

  }
}