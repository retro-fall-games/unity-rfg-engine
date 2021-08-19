using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Checkpoints/Checkpoint")]
  public class Checkpoint : MonoBehaviour
  {
    [Header("Settings")]
    public int Index = 0;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        CheckpointManager.Instance.HitNewCheckpoint(Index);
      }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      UnityEditor.Handles.color = Color.yellow;
      UnityEditor.Handles.Label(transform.position, $"Checkpoint {Index}");
    }
#endif

  }
}