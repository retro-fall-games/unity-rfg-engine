using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG;
using RFG.Platformer;

namespace Game
{
  [AddComponentMenu("Game/Profile/Profile Manager")]
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    [Header("Settings")]
    public Profile Profile;
    public bool OverrideProfile;

    [HideInInspector]
    private Character character;
    private CharacterAbilityController abilityController;
    private Inventory inventory;
    private EquipmentSet equipmentSet;

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player != null)
      {
        character = player.GetComponent<Character>();
        abilityController = player.GetComponent<CharacterAbilityController>();
        inventory = player.GetComponent<Inventory>();
        equipmentSet = player.GetComponent<EquipmentSet>();
      }
      if (OverrideProfile)
      {
        if (Profile.CreatedAt == 0)
        {
          Profile.Create();
        }
        else
        {
          Profile.Load();
        }
        SelectProfile(Profile);
      }
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
      Profile.StartedAt = Epoch.Current();
    }

    public void SelectProfile(Profile profile)
    {
      Profile = profile;
      PlayerPrefs.SetInt("startingCheckpoint", Profile.CheckpointIndex);
      RestoreData();
    }

    public void SaveProfile()
    {
      Profile.Level = SceneManager.Instance.GetCurrentScene();
      Profile.CheckpointIndex = CheckpointManager.Instance.CurrentCheckpointIndex;
      // if (abilityController != null)
      // {
      //   Profile.Abilities = abilityController.Abilities.FindGuids();
      // }
      if (inventory != null)
      {
        Profile.Inventory = inventory.GetSave();
      }
      if (equipmentSet != null)
      {
        Profile.EquipmentSet = equipmentSet.GetSave();
      }
      Profile.Save();
    }

    public void RestoreData()
    {
      if (inventory != null)
      {
        inventory.RestoreSave(Profile.Inventory);
      }
      if (equipmentSet != null)
      {
        equipmentSet.RestoreSave(Profile.EquipmentSet);
      }
    }

    private void OnEnable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
  }
}