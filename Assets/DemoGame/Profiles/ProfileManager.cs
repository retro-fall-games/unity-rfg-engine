using System.Collections;
using UnityEngine;
using RFG.Core;
using RFG.Platformer;
using RFG.SceneGraph;
using RFG.Items;
using RFG.Interactions;

namespace Game
{
  [AddComponentMenu("Game/Profile/Profile Manager")]
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    [Header("Settings")]
    public Profile Profile;
    public bool OverrideProfile;

    [Header("Event Observers")]
    public ObserverString[] SaveProfileObservers;

    [HideInInspector]
    private Character character;
    private Inventory inventory;
    private EquipmentSet equipmentSet;
    private JumpAbility jumpAbility;
    private DashAbility dashAbility;
    private WallClingingAbility wallClingingAbility;
    private WallJumpAbility wallJumpAbility;

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
        StartCoroutine(LoadData());
      }
    }

    public void SelectProfile(Profile profile)
    {
      Profile = profile;
      CheckpointManager.Instance.SetStartingCheckpoint(Profile.CheckpointIndex);
    }

    public void SaveProfile(string scene = null)
    {
      SaveProfile();
    }

    public void SaveProfile()
    {
      Profile.Level = SceneManager.Instance.GetCurrentScene();
      Profile.CheckpointIndex = CheckpointManager.Instance.CurrentCheckpointIndex;
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

    public IEnumerator LoadData()
    {
      yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player != null)
      {
        character = player.GetComponent<Character>();
        inventory = player.GetComponent<Inventory>();
        equipmentSet = player.GetComponent<EquipmentSet>();
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

        // If the item is in inventory, if they do, the turn it off
        if (item is AbilityItem abilityItem)
        {
          if (inventory.InInventory(abilityItem.Guid))
          {
            pickUp.gameObject.SetActive(false);
          }
        }
        if (item is MaxHealthItem maxHealthItem)
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
      foreach (ObserverString observer in SaveProfileObservers)
      {
        observer.OnRaise += SaveProfile;
      }
    }

    private void OnDisable()
    {
      UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
      foreach (ObserverString observer in SaveProfileObservers)
      {
        observer.OnRaise -= SaveProfile;
      }
    }
  }
}