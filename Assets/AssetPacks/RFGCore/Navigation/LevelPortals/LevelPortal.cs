using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Navigation/Level Portals/Level Portal")]
  [RequireComponent(typeof(BoxCollider2D))]
  public class LevelPortal : MonoBehaviour
  {
    [Header("Settings")]
    public int index = 0;

    [Header("Next Level Setting")]
    public string toScene = "";
    public int toLevelPortalIndex = -1;
    public float waitForSeconds = 0f;

    [Header("Soundtrack")]
    public bool fadeSoundtrack = false;

    [HideInInspector]
    public bool JustWarped { get; set; }

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (!JustWarped && toLevelPortalIndex != -1 && !toScene.Equals(""))
        {
          EventManager.TriggerEvent(new LevelPortalEvent(toScene, toLevelPortalIndex, fadeSoundtrack, waitForSeconds));
        }
      }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
      JustWarped = false;
    }

  }
}