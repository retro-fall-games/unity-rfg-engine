using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Level Portals/Level Portal")]
  public class LevelPortal : MonoBehaviour
  {
    [Header("Settings")]
    public int Index = 0;

    [Header("Next Level Setting")]
    public string ToScene = "";
    public int ToLevelPortalIndex = -1;
    public Vector3 SpawnOffset = Vector2.zero;

    [Header("Event Observer")]
    public ObserverString LevelPortalObserver;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (ToLevelPortalIndex != -1 && !ToScene.Equals(""))
        {
          PlayerPrefs.SetInt("levelPortalTo", ToLevelPortalIndex);
          LevelPortalObserver.Raise(ToScene);
          // SceneManager.Instance.LoadScene(ToScene);
        }
      }
    }

  }
}