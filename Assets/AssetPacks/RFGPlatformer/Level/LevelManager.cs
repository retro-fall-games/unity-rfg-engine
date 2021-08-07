using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG Platformer/Level/Level Manager")]
    public class LevelManager : Singleton<LevelManager>
    {

      [Header("Player Prefab")]
      public GameObject player;

      [HideInInspector]
      public bool Loaded { get; set; }
      public Character PlayerCharacter => _character;
      private GameObject _playerInstance;
      private CinemachineVirtualCamera _virtualCam;
      private Transform _playerTransform;
      private Dictionary<int, LevelPortal> _levelPortals;
      private Character _character;

      protected override void Awake()
      {
        base.Awake();
        _virtualCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

        // Cache Level Portals By Index
        _levelPortals = new Dictionary<int, LevelPortal>();
        GameObject[] levelPortals = GameObject.FindGameObjectsWithTag("Level Portal");
        foreach (GameObject levelPortal in levelPortals)
        {
          LevelPortal _levelPortal = levelPortal.GetComponent<LevelPortal>();
          if (_levelPortal)
          {
            int index = _levelPortal.index;
            _levelPortals.Add(index, _levelPortal);
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



      private void CreatePlayer()
      {
        // Create at the current checkpoint
        Vector3 spawnAt = CheckpointManager.Instance.CurrentCheckpoint.position;
        _playerInstance = Instantiate(player, spawnAt, Quaternion.identity);
        _playerInstance.SetActive(false);
        _playerTransform = _playerInstance.transform;
        _virtualCam.Follow = _playerTransform;

        _character = _playerInstance.GetComponent<Character>();
        SpawnPlayer();


      }

      public void SpawnPlayer()
      {
        _playerTransform.position = CheckpointManager.Instance.CurrentCheckpoint.position;
        // _character.Birth();
      }

      public void KillPlayer()
      {
        // _character.Kill();
      }

      public void OnEvent(LevelPortalEvent levelPortalEvent)
      {
        PlayerPrefs.SetInt("levelPortalTo", levelPortalEvent.toLevelPortalIndex);
        // SceneManager.Instance.LoadScene(levelPortalEvent.toScene, levelPortalEvent.fadeSoundtrack, levelPortalEvent.waitForSeconds);
      }

    }
  }
}