using System.Linq;
using UnityEngine;

namespace RFG
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
    public Transform[] paths;
    public bool ReachedEnd { get; private set; }
    public bool ReachedStart { get; private set; }
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
      if (autoMove)
      {
        Move();
      }
    }

    public void Move()
    {
      CheckPath();
      _transform.position = Vector2.MoveTowards(_transform.position, NextPath.position, speed * Time.deltaTime);
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
      System.Array.Reverse(paths);
    }

    private void SetNextPath()
    {
      int nextIndex = direction == Direction.Forwards ? _nextIndex + 1 : _nextIndex - 1;
      ReachedEnd = nextIndex >= paths.Length;
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
        nextIndex = paths.Length - 1;
      }
      else if (ReachedEnd)
      {
        nextIndex = paths.Length - 1;
      }
      else if (ReachedStart)
      {
        nextIndex = 0;
      }
      _nextIndex = nextIndex;

    }

    private void OnDrawGizmos()
    {
      if (paths == null || paths.Length < 2)
      {
        return;
      }
      var pathsList = paths.Where(t => t != null).ToList();

      for (var i = 1; i < pathsList.Count; i++)
      {
        Gizmos.DrawLine(paths[i - 1].position, paths[i].position);
      }
    }

  }
}