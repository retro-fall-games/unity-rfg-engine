using UnityEngine;

namespace RFG.Interactions
{
  [AddComponentMenu("RFG/Interactions/Checkpoints/Checkpoint")]
  public class Checkpoint : MonoBehaviour
  {
    [Header("Settings")]
    public int Index = 0;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (CheckpointManager.Instance.NewLevel || Index != CheckpointManager.Instance.CurrentCheckpointIndex)
        {
          CheckpointManager.Instance.HitNewCheckpoint(Index);
        }
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