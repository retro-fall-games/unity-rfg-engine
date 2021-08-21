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
      if (Profile != null)
      {
        Profile.StartedAt = Epoch.Current();
        StartCoroutine(RestoreData());
      }
    }

    public void SelectProfile(Profile profile)
    {
      Profile = profile;
      CheckpointManager.Instance.SetStartingCheckpoint(Profile.CheckpointIndex);
    }

    public void SaveProfile()
    {
      Profile.Level = SceneManager.Instance.GetCurrentScene();
      Profile.CheckpointIndex = CheckpointManager.Instance.CurrentCheckpointIndex;
      if (abilityController != null)
      {
        Profile.Abilities = abilityController.GetSave();
      }
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

    public IEnumerator RestoreData()
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
      if (abilityController != null)
      {
        abilityController.RestoreSave(Profile.Abilities);
      }
      if (inventory != null)
      {
        inventory.RestoreSave(Profile.Inventory);
      }
      if (equipmentSet != null)
      {
        equipmentSet.RestoreSave(Profile.EquipmentSet);
      }

      // Go through all the pickups and turn of the Ability Pickups if the player already has them
      PickUp[] pickUps = GameObject.FindObjectsOfType<PickUp>();
      foreach (PickUp pickUp in pickUps)
      {
        Item item = pickUp.item;

        // If the item is an ability then check if they alreay have it, if they do, the turn it off
        if (item is AbilityItem abilityItem)
        {
          foreach (CharacterAbility ability in abilityItem.AbilitiesToAdd)
          {
            if (abilityController.HasAbility(ability))
            {
              pickUp.gameObject.SetActive(false);
            }
          }
        }
        else if (item is MaxHealthItem maxHealthItem)
        {
          if (inventory.InInventory(maxHealthItem.Guid))
          {
            pickUp.gameObject.SetActive(false);
          }
        }
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