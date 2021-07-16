
using UnityEngine;
using System.Collections.Generic;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Navigation/Checkpoints/Checkpoint Manager")]
  public class CheckpointManager : Singleton<CheckpointManager>, EventListener<CheckpointEvent>
  {

    [Header("Checkpoint Config")]
    public int startingCheckpoint = 0;
    public int currentCheckpointIndex = 0;
    public Transform CurrentCheckpoint => _currentCheckpoint;

    [HideInInspector]
    private List<Transform> _checkpoints = new List<Transform>();
    private Transform _currentCheckpoint;

    protected override void Awake()
    {
      base.Awake();
      GameObject[] checkpointsGameObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
      foreach (GameObject checkpointGameObject in checkpointsGameObjects)
      {
        Transform checkpointTransform = checkpointGameObject.transform;
        Checkpoint checkpoint = checkpointGameObject.GetComponent<Checkpoint>();
        if (checkpoint)
        {
          int index = checkpoint.index;
          _checkpoints.Insert(index, checkpointTransform);
        }
      }
      SetCurrentCheckpoint(startingCheckpoint);
    }

    public void SetCurrentCheckpoint(int index)
    {
      if (index < _checkpoints.Count)
      {
        currentCheckpointIndex = index;
        _currentCheckpoint = _checkpoints[currentCheckpointIndex];
      }
    }

    public void OnEvent(CheckpointEvent checkpointEvent)
    {
      SetCurrentCheckpoint(checkpointEvent.checkpointIndex);
    }

    private void OnEnable()
    {
      this.AddListener<CheckpointEvent>();
    }

    private void OnDisable()
    {
      this.RemoveListener<CheckpointEvent>();
    }

  }
}