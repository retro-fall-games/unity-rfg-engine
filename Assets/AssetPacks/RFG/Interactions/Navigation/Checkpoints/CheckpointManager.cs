
using UnityEngine;
using System.Collections.Generic;

namespace RFG
{
  [AddComponentMenu("RFG/Navigation/Checkpoints/Checkpoint Manager")]
  public class CheckpointManager : Singleton<CheckpointManager>
  {

    [Header("Checkpoint Config")]
    public int StartingCheckpoint = 0;
    public int CurrentCheckpointIndex = 0;
    public bool OverrideStartingCheckpoint = false;
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
          int index = checkpoint.Index;
          _checkpoints.Insert(index, checkpointTransform);
        }
      }
      // So if we haven't modified the starting checkpoint and we have a startingCheckpoint
      // from player prefs, then use that
      if (!OverrideStartingCheckpoint && StartingCheckpoint == 0 && PlayerPrefs.HasKey("startingCheckpoint"))
      {
        StartingCheckpoint = PlayerPrefs.GetInt("startingCheckpoint", 0);
      }
      SetCurrentCheckpoint(StartingCheckpoint);
    }

    public void SetCurrentCheckpoint(int index)
    {
      if (index < _checkpoints.Count)
      {
        CurrentCheckpointIndex = index;
        _currentCheckpoint = _checkpoints[CurrentCheckpointIndex];
      }
    }

  }
}