using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Checkpoints/Checkpoint")]
  [RequireComponent(typeof(BoxCollider2D))]
  public class Checkpoint : MonoBehaviour
  {
    public int Index = 0;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (Index != CheckpointManager.Instance.CurrentCheckpointIndex)
        {
          CheckpointManager.Instance.SetCurrentCheckpoint(Index);
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