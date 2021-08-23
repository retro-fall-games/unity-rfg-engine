
using UnityEngine;
using System.Collections.Generic;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Checkpoints/Checkpoint Manager")]
  public class CheckpointManager : Singleton<CheckpointManager>
  {

    [Header("Checkpoint Config")]
    public int StartingCheckpoint = 0;
    public int CurrentCheckpointIndex { get; private set; }
    public bool NewLevel { get; set; }
    public bool OverrideStartingCheckpoint = false;
    public Transform CurrentCheckpoint => _currentCheckpoint;

    [Header("Game Events")]
    public GameEvent CheckpointEvent;

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
          int index = checkpoint.Index;
          _checkpoints.Insert(index, checkpointTransform);
        }
      }
      SetCurrentCheckpoint(StartingCheckpoint);
      NewLevel = true;
    }

    public void SetStartingCheckpoint(int index)
    {
      if (!OverrideStartingCheckpoint)
      {
        SetCurrentCheckpoint(index);
      }
    }

    public void SetCurrentCheckpoint(int index)
    {
      if (index >= 0 && index < _checkpoints.Count)
      {
        CurrentCheckpointIndex = index;
        _currentCheckpoint = _checkpoints[CurrentCheckpointIndex];
      }
    }

    public void HitNewCheckpoint(int index)
    {
      NewLevel = false;
      SetCurrentCheckpoint(index);
      CheckpointEvent?.Raise();
    }

  }
}