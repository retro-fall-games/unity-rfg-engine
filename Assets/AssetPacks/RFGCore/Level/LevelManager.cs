using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RFG
{
  [AddComponentMenu("RFG Engine/Level/Level Manager")]
  public class LevelManager : Singleton<LevelManager>, EventListener<LevelPortalEvent>
  {
    public enum BoundsBehavior { Nothing, Constrain, Kill }

    [Header("Player")]
    public GameObject player;

    [Header("Level Bounds")]
    public BoundsBehavior top = BoundsBehavior.Constrain;
    public BoundsBehavior bottom = BoundsBehavior.Kill;
    public BoundsBehavior left = BoundsBehavior.Constrain;
    public BoundsBehavior right = BoundsBehavior.Constrain;
    public Bounds levelBounds = new Bounds(Vector3.zero, Vector3.one * 10);

    [HideInInspector]
    public bool Loaded { get; set; }
    private GameObject _playerInstance;
    private GameObject cmCam;
    private CinemachineVirtualCamera _virtualCam;
    private bool _loaded = false;
    private Character _character;
    private Transform _playerTransform;
    private Vector2 _constrainedPosition = Vector2.zero;

    private List<LevelPortal> _levelPortals = new List<LevelPortal>();

    protected override void Awake()
    {
      base.Awake();
      _virtualCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
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
      // Wait until everything is loaded
      yield return new WaitUntil(() => SceneManager.Instance != null);
      yield return new WaitUntil(() => CheckpointManager.Instance != null);
      yield return new WaitUntil(() => CheckpointManager.Instance.CurrentCheckpoint != null);

      yield return CreatePlayer();
      _character = _playerInstance.GetComponent<Character>();
      yield return new WaitUntil(() => _character.Controller != null);
      _character.Birth();

      Loaded = true;

      yield break;
    }

    private void LateUpdate()
    {
      // Only process when the level has loaded
      if (!_loaded)
      {
        return;
      }

      // Only process when the player is alive
      if (_character.CharacterState.CurrentState == CharacterStates.Dead)
      {
        return;
      }

      HandleLevelBounds();
    }

    private IEnumerator CreatePlayer()
    {
      Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;
      _playerInstance = Instantiate(player, spawnAt, Quaternion.identity);
      _playerTransform = _playerInstance.transform;
      _virtualCam.Follow = _playerTransform;

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

      yield break;
    }

    private void SpawnPlayer()
    {
      _playerTransform.position = CheckpointManager.Instance.CurrentCheckpoint.position;
      _character.Birth();
    }

    private void HandleLevelBounds()
    {
      if (levelBounds.size != Vector3.zero)
      {
        if (top != BoundsBehavior.Nothing && _character.Controller.ColliderTopPosition.y > levelBounds.max.y)
        {
          _constrainedPosition.x = _playerTransform.position.x;
          _constrainedPosition.y = levelBounds.max.y - Mathf.Abs(_character.Controller.ColliderSize.y) / 2;
          ApplyBoundsBehavior(top, _constrainedPosition);
        }

        if (bottom != BoundsBehavior.Nothing && _character.Controller.ColliderBottomPosition.y < levelBounds.min.y)
        {
          _constrainedPosition.x = _playerTransform.position.x;
          _constrainedPosition.y = levelBounds.min.y + Mathf.Abs(_character.Controller.ColliderSize.y) / 2;
          ApplyBoundsBehavior(bottom, _constrainedPosition);
        }

        if (right != BoundsBehavior.Nothing && _character.Controller.ColliderRightPosition.x > levelBounds.max.x)
        {
          _constrainedPosition.x = levelBounds.max.x - Mathf.Abs(_character.Controller.ColliderSize.x) / 2;
          _constrainedPosition.y = _playerTransform.position.y;
          ApplyBoundsBehavior(right, _constrainedPosition);
        }

        if (left != BoundsBehavior.Nothing && _character.Controller.ColliderLeftPosition.x < levelBounds.min.x)
        {
          _constrainedPosition.x = levelBounds.min.x + Mathf.Abs(_character.Controller.ColliderSize.x) / 2;
          _constrainedPosition.y = _playerTransform.position.y;
          ApplyBoundsBehavior(left, _constrainedPosition);
        }
      }
    }

    private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
    {
      if (behavior == BoundsBehavior.Kill)
      {
        KillPlayer();
      }
      else if (behavior == BoundsBehavior.Constrain)
      {
        _playerTransform.position = constrainedPosition;
      }
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

    private void KillPlayer()
    {
      StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()
    {
      _character.Kill();
      yield return new WaitForSeconds(1f);
      SpawnPlayer();
    }

    public void OnEvent(LevelPortalEvent levelPortalEvent)
    {
      PlayerPrefs.SetInt("levelPortalTo", levelPortalEvent.toLevelPortalIndex);
      SceneManager.Instance.LoadScene(levelPortalEvent.toScene, levelPortalEvent.waitForSeconds);
    }

    private void OnEnable()
    {
      this.AddListener<LevelPortalEvent>();
    }

    private void OnDisable()
    {
      this.RemoveListener<LevelPortalEvent>();
    }

  }
}
