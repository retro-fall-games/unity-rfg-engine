using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RFG.Navigation
{
  [AddComponentMenu("RFG/Navigation/Paths/Movement Path")]
  public class MovementPath : MonoBehaviour
  {
    public enum State { Loop, PingPong, OneWay };
    public enum Direction { Forwards, Backwards };

    [Header("Parameters")]
    public State state = State.OneWay;
    public Direction direction = Direction.Forwards;
    public float speed = 3f;
    public bool spawnAtStart = false;
    public bool autoMove = false;
    public bool Paused = false;
    public List<Transform> paths = new List<Transform>();
    public bool ReachedEnd { get; private set; }
    public bool ReachedStart { get; private set; }
    public Vector2 CurrentSpeed { get; private set; }
    public Transform NextPath
    {
      get
      {
        Transform nextPath = paths[_nextIndex];
        Vector2 nextPostion = nextPath.position;
        if (LockY)
        {
          nextPostion.y = _transform.position.y;
        }
        nextPath.position = nextPostion;
        return nextPath;
      }
    }

    [Header("Constraints")]
    public bool LockY;

    [HideInInspector]
    private int _nextIndex = 0;
    private Transform _transform;

    private void Awake()
    {
      _transform = transform;
    }

    private void Start()
    {
      if (spawnAtStart)
      {
        _transform.position = NextPath.position;
        _nextIndex++;
      }
    }

    private void Update()
    {
      if (autoMove && !Paused)
      {
        Move();
      }
    }

    public void TogglePause()
    {
      Paused = !Paused;
    }

    public void Move()
    {
      CheckPath();
      CurrentSpeed = Vector2.MoveTowards(_transform.position, NextPath.position, speed * Time.deltaTime);
      _transform.position = CurrentSpeed;
    }

    public void CheckPath()
    {
      int range = (int)Vector2.Distance(_transform.position, NextPath.position);
      if (range == 0f)
      {
        SetNextPath();
      }
    }

    public void Reset()
    {
      ReachedEnd = false;
      ReachedStart = false;
    }

    public void Reverse()
    {
      paths.Reverse();
    }

    private void SetNextPath()
    {
      int nextIndex = direction == Direction.Forwards ? _nextIndex + 1 : _nextIndex - 1;
      ReachedEnd = nextIndex >= paths.Count;
      ReachedStart = nextIndex < 0;

      if (state == State.PingPong && (ReachedEnd || ReachedStart))
      {
        switch (direction)
        {
          case Direction.Forwards:
            direction = Direction.Backwards;
            nextIndex--;
            break;
          case Direction.Backwards:
            direction = Direction.Forwards;
            nextIndex++;
            break;
          default:
            break;
        }
      }
      else if (state == State.Loop && ReachedEnd)
      {
        nextIndex = 0;
      }
      else if (state == State.Loop && ReachedStart)
      {
        nextIndex = paths.Count - 1;
      }
      else if (ReachedEnd)
      {
        nextIndex = paths.Count - 1;
      }
      else if (ReachedStart)
      {
        nextIndex = 0;
      }
      _nextIndex = nextIndex;

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
      if (paths == null || paths.Count < 2)
      {
        return;
      }
      var pathsList = paths.Where(t => t != null).ToList();

      for (var i = 1; i < pathsList.Count; i++)
      {
        Gizmos.DrawLine(paths[i - 1].position, paths[i].position);
      }
    }


    [ButtonMethod]
    private void CreatePath()
    {
      string path = "Assets/AssetPacks/RFG/Navigation/Paths/Prefabs";
      string objName = "Path";
      Object obj = AssetDatabase.LoadAssetAtPath($"{path}/{objName}.prefab", typeof(GameObject));
      GameObject clone = Instantiate(obj) as GameObject;
      clone.name = objName;
      GameObject parentObj = GameObject.Find("Navigation");
      clone.transform.SetParent(parentObj.transform);

      foreach (Transform t in clone.transform)
      {
        paths.Add(t);
      }
      EditorUtility.SetDirty(transform);
    }
#endif

  }
}