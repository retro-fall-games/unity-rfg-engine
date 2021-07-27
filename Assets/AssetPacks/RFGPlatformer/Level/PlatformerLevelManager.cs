using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Managers/Platformer Level Manager")]
  public class PlatformerLevelManager : Singleton<PlatformerLevelManager>, EventListener<LevelPortalEvent>
  {
    public enum BoundsBehaviour { Nothing, Constrain, Kill }

    [Header("Player Prefab")]
    public GameObject player;

    [Header("Level Bounds")]
    public BoundsBehaviour top = BoundsBehaviour.Constrain;
    public BoundsBehaviour bottom = BoundsBehaviour.Kill;
    public BoundsBehaviour left = BoundsBehaviour.Constrain;
    public BoundsBehaviour right = BoundsBehaviour.Constrain;
    public Bounds levelBounds = new Bounds(Vector3.zero, Vector3.one * 10);

    [HideInInspector]
    public bool Loaded { get; set; }
    public PlatformerCharacter PlayerCharacter => _character;
    private GameObject _playerInstance;
    private CinemachineVirtualCamera _virtualCam;
    private Transform _playerTransform;
    private Vector2 _constrainedPosition = Vector2.zero;
    private List<LevelPortal> _levelPortals = new List<LevelPortal>();
    private PlatformerCharacter _character;

    protected override void Awake()
    {
      base.Awake();
      _virtualCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

      // Cache Level Portals By Index
      GameObject[] levelPortals = GameObject.FindGameObjectsWithTag("Level Portal");
      foreach (GameObject levelPortal in levelPortals)
      {
        LevelPortal _levelPortal = levelPortal.GetComponent<LevelPortal>();
        if (_levelPortal)
        {
          int index = _levelPortal.index;
          _levelPortals.Insert(index, _levelPortal);
        }
      }
    }

    private void Start()
    {
      Loaded = false;
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => SceneManager.Instance != null);
      yield return new WaitUntil(() => CheckpointManager.Instance != null);
      yield return new WaitUntil(() => CheckpointManager.Instance.CurrentCheckpoint != null);

      CreatePlayer();
      yield return new WaitUntil(() => _character != null);

      Loaded = true;

      yield break;
    }

    private void LateUpdate()
    {
      // Only process when the level has loaded
      if (!Loaded)
      {
        return;
      }

      if (_character.CharacterState.CurrentState == CharacterStates.Dead)
      {
        return;
      }

      HandleLevelBounds();
    }

    private void CreatePlayer()
    {
      // Create at the current checkpoint
      Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;
      _playerInstance = Instantiate(player, spawnAt, Quaternion.identity);
      _playerInstance.SetActive(false);
      _playerTransform = _playerInstance.transform;
      _virtualCam.Follow = _playerTransform;

      _character = _playerInstance.GetComponent<PlatformerCharacter>();
      SpawnPlayer();

      // If coming from a level portal the warp the player to that portal
      int levelPortalTo = PlayerPrefs.GetInt("levelPortalTo", -1);
      if (levelPortalTo != -1)
      {
        if (levelPortalTo >= 0 && levelPortalTo <= _levelPortals.Count)
        {
          LevelPortal levelPortal = _levelPortals[levelPortalTo];
          levelPortal.JustWarped = true;
          _playerInstance.transform.position = levelPortal.transform.position;
          PlayerPrefs.SetInt("levelPortalTo", -1);
        }
      }
    }

    public void SpawnPlayer()
    {
      _playerTransform.position = CheckpointManager.Instance.CurrentCheckpoint.position;
      _character.Birth();
    }

    private void HandleLevelBounds()
    {
      if (levelBounds.size != Vector3.zero)
      {
        if (top != BoundsBehaviour.Nothing && _character.Controller.ColliderTopPosition.y > levelBounds.max.y)
        {
          _constrainedPosition.x = _playerTransform.position.x;
          _constrainedPosition.y = levelBounds.max.y - Mathf.Abs(_character.Controller.ColliderSize.y) / 2;
          ApplyBoundsBehaviour(top, _constrainedPosition);
        }

        if (bottom != BoundsBehaviour.Nothing && _character.Controller.ColliderBottomPosition.y < levelBounds.min.y)
        {
          _constrainedPosition.x = _playerTransform.position.x;
          _constrainedPosition.y = levelBounds.min.y + Mathf.Abs(_character.Controller.ColliderSize.y) / 2;
          ApplyBoundsBehaviour(bottom, _constrainedPosition);
        }

        if (right != BoundsBehaviour.Nothing && _character.Controller.ColliderRightPosition.x > levelBounds.max.x)
        {
          _constrainedPosition.x = levelBounds.max.x - Mathf.Abs(_character.Controller.ColliderSize.x) / 2;
          _constrainedPosition.y = _playerTransform.position.y;
          ApplyBoundsBehaviour(right, _constrainedPosition);
        }

        if (left != BoundsBehaviour.Nothing && _character.Controller.ColliderLeftPosition.x < levelBounds.min.x)
        {
          _constrainedPosition.x = levelBounds.min.x + Mathf.Abs(_character.Controller.ColliderSize.x) / 2;
          _constrainedPosition.y = _playerTransform.position.y;
          ApplyBoundsBehaviour(left, _constrainedPosition);
        }
      }
    }

    private void ApplyBoundsBehaviour(BoundsBehaviour Behaviour, Vector2 constrainedPosition)
    {
      if (Behaviour == BoundsBehaviour.Kill)
      {
        KillPlayer();
      }
      else if (Behaviour == BoundsBehaviour.Constrain)
      {
        _playerTransform.position = constrainedPosition;
      }
    }

    public void KillPlayer()
    {
      _character.Kill();
    }

    public void OnEvent(LevelPortalEvent levelPortalEvent)
    {
      PlayerPrefs.SetInt("levelPortalTo", levelPortalEvent.toLevelPortalIndex);
      SceneManager.Instance.LoadScene(levelPortalEvent.toScene, levelPortalEvent.fadeSoundtrack, levelPortalEvent.waitForSeconds);
    }

    private void OnEnable()
    {
      this.AddListener<LevelPortalEvent>();
    }

    private void OnDisable()
    {
      this.RemoveListener<LevelPortalEvent>();
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void GeneratePolygonCollider2D()
    {
      PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
      Vector2[] points = new Vector2[]
      {
        new Vector2(levelBounds.min.x, levelBounds.min.y),
        new Vector2(levelBounds.min.x, levelBounds.max.y),
        new Vector2(levelBounds.max.x, levelBounds.max.y),
        new Vector2(levelBounds.max.x, levelBounds.min.y),
      };
      collider.SetPath(0, points);
    }

    private void OnDrawGizmos()
    {
      var b = levelBounds;
      var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
      var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
      var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
      var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

      Gizmos.DrawLine(p1, p2);
      Gizmos.DrawLine(p2, p3);
      Gizmos.DrawLine(p3, p4);
      Gizmos.DrawLine(p4, p1);

      // top
      var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
      var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
      var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
      var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

      Gizmos.DrawLine(p5, p6);
      Gizmos.DrawLine(p6, p7);
      Gizmos.DrawLine(p7, p8);
      Gizmos.DrawLine(p8, p5);

      // sides
      Gizmos.DrawLine(p1, p5);
      Gizmos.DrawLine(p2, p6);
      Gizmos.DrawLine(p3, p7);
      Gizmos.DrawLine(p4, p8);
    }
#endif

  }
}
