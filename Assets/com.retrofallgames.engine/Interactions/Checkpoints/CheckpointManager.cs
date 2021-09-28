
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RFG.Interactions
{
  using Core;

  [AddComponentMenu("RFG/Interactions/Checkpoints/Checkpoint Manager")]
  public class CheckpointManager : Singleton<CheckpointManager>
  {

    [Header("Checkpoint Config")]
    public int StartingCheckpoint = 0;
    public int CurrentCheckpointIndex { get; private set; }
    public bool NewLevel { get; set; }
    public bool OverrideStartingCheckpoint = false;
    public Transform CurrentCheckpoint => _currentCheckpoint;

    [Header("Event Observer")]
    public ObserverString CheckpointObserver;

    [HideInInspector]
    private List<Transform> _checkpoints = new List<Transform>();
    private Transform _currentCheckpoint;

    protected override void Awake()
    {
      base.Awake();
      GameObject[] checkpointsGameObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
      List<Checkpoint> checkpoints = new List<Checkpoint>();
      foreach (GameObject checkpointGameObject in checkpointsGameObjects)
      {
        Checkpoint checkpoint = checkpointGameObject.GetComponent<Checkpoint>();
        if (checkpoint)
        {
          checkpoints.Add(checkpoint);
        }
      }

      checkpoints = checkpoints.OrderBy(x => x.Index).ToList<Checkpoint>();
      foreach (Checkpoint checkpoint in checkpoints)
      {
        if (checkpoint)
        {
          _checkpoints.Insert(checkpoint.Index, checkpoint.transform);
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
      CheckpointObserver?.Raise("");
    }

  }
}