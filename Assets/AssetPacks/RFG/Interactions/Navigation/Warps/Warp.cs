using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Warps/Warp")]
  [RequireComponent(typeof(BoxCollider2D))]
  public class Warp : MonoBehaviour
  {
    [Header("Settings")]
    public int index = 0;
    public int warpToIndex = 0;

    [Header("Audio")]
    public string[] soundFx;
    public bool soundFxFade;

    [HideInInspector]
    private bool JustWarped { get; set; }
    private List<Warp> _warps = new List<Warp>();

    private void Awake()
    {
      GameObject[] warps = GameObject.FindGameObjectsWithTag("Warp");
      foreach (GameObject warp in warps)
      {
        Warp _warp = warp.GetComponent<Warp>();
        if (_warp)
        {
          int index = _warp.index;
          _warps.Insert(index, _warp);
        }
      }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (!JustWarped && index != warpToIndex && warpToIndex >= 0 && warpToIndex < _warps.Count)
        {
          Warp warpTo = _warps[warpToIndex];
          warpTo.JustWarped = true;
          JustWarped = true;
          col.gameObject.transform.position = warpTo.transform.position;
          // EventManager.TriggerEvent(new WarpEvent(index, warpToIndex));
          if (soundFx != null && soundFx.Length > 0)
          {
            // FXAudio.Instance.Play(soundFx, soundFxFade);
          }
        }
      }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
      JustWarped = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      UnityEditor.Handles.color = Color.yellow;
      UnityEditor.Handles.Label(transform.position, $"Warp {index}");
    }
#endif 

  }
}