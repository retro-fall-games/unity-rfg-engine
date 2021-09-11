using System.Collections;
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
      if (character != null)
      {
        if (character.Context.settingsPack.JumpSettings.NumberOfJumps == 2)
        {
          Profile.HasDoubleJump = true;
        }
        else
        {
          Profile.HasDoubleJump = false;
        }
      }
      if (dashAbility != null)
      {
        Profile.HasDash = dashAbility.HasAbility;
      }
      if (wallJumpAbility != null)
      {
        Profile.HasWallJump = wallJumpAbility.HasAbility;
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
        jumpAbility = player.GetComponent<JumpAbility>();
        dashAbility = player.GetComponent<DashAbility>();
        wallClingingAbility = player.GetComponent<WallClingingAbility>();
        wallJumpAbility = player.GetComponent<WallJumpAbility>();
      }
      if (inventory != null)
      {
        inventory.RestoreSave(Profile.Inventory);
      }
      if (equipmentSet != null)
      {
        equipmentSet.RestoreSave(Profile.EquipmentSet);
      }
      if (jumpAbility != null)
      {
        if (Profile.HasDoubleJump)
        {
          character.Context.settingsPack.JumpSettings.NumberOfJumps = 2;
        }
        else
        {
          character.Context.settingsPack.JumpSettings.NumberOfJumps = 1;
        }
      }
      if (dashAbility != null)
      {
        if (Profile.HasDash)
        {
          dashAbility.HasAbility = true;
        }
        else
        {
          dashAbility.HasAbility = false;
        }
      }
      if (wallClingingAbility != null && wallJumpAbility != null)
      {
        if (Profile.HasWallJump)
        {
          wallClingingAbility.HasAbility = true;
          wallJumpAbility.HasAbility = true;
        }
        else
        {
          wallClingingAbility.HasAbility = false;
          wallJumpAbility.HasAbility = false;
        }
      }

      // Go through all the pickups and turn of the Ability Pickups if the player already has them
      PickUp[] pickUps = GameObject.FindObjectsOfType<PickUp>();
      foreach (PickUp pickUp in pickUps)
      {
        Item item = pickUp.item;

        // If the item is an ability then check if they alreay have it, if they do, the turn it off
        if (item is AbilityItem abilityItem)
        {
          switch (abilityItem.AbilityToAdd)
          {
            case AbilityItem.AbilityType.DoubleJump:
              if (Profile.HasDoubleJump)
              {
                pickUp.gameObject.SetActive(false);
              }
              break;
            case AbilityItem.AbilityType.Dash:
              if (Profile.HasDash)
              {
                pickUp.gameObject.SetActive(false);
              }
              break;
            case AbilityItem.AbilityType.WallJump:
              if (Profile.HasWallJump)
              {
                pickUp.gameObject.SetActive(false);
              }
              break;
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