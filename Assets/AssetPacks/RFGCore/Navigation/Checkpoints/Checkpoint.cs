using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Navigation/Checkpoints/Checkpoint")]
  [RequireComponent(typeof(BoxCollider2D))]
  public class Checkpoint : MonoBehaviour
  {
    public int index = 0;

    public void OnTriggerEnter2D(Collider2D col)
    {
      if (col.gameObject.CompareTag("Player"))
      {
        if (index != CheckpointManager.Instance.currentCheckpointIndex)
        {
          EventManager.TriggerEvent(new CheckpointEvent(index));
        }
      }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      UnityEditor.Handles.color = Color.yellow;
      UnityEditor.Handles.Label(transform.position, $"Checkpoint {index}");
    }
#endif

  }
}