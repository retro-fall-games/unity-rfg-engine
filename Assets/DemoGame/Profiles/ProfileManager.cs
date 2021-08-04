using UnityEngine;
using RFG;

namespace Game
{
  public class ProfileManager : PersistentSingleton<ProfileManager>, EventListener<CheckpointEvent>
  {
    public int loadProfileId = -1;
    private Profile<Game.ProfileData> _profile;

    protected override void Awake()
    {
      base.Awake();
      // This is here for debugging purposes, this wont be used in a real game
      if (loadProfileId > -1)
      {
        // LogExt.Log<ProfileManager>("Loading profile: " + loadProfileId);
        _profile = new Profile<Game.ProfileData>();
        _profile.Load(loadProfileId);
        if (_profile.data != null)
        {
          PlayerPrefs.SetInt("startingCheckpoint", _profile.data.checkpoint);
        }
        else
        {
          LogExt.Warn<ProfileManager>("Did not load profile");
        }
      }
    }
    public void SetProfile(Profile<Game.ProfileData> profile)
    {
      _profile = profile;
    }

    public Profile<ProfileData> GetProfile()
    {
      return _profile;
    }

    public void SaveProfile()
    {
      _profile.Save();
    }

    public void OnEvent(CheckpointEvent checkpointEvent)
    {
      if (_profile != null && _profile.data != null)
      {
        _profile.data.level = SceneManager.Instance.GetCurrentScene();
        _profile.data.checkpoint = checkpointEvent.checkpointIndex;

        PlatformerCharacter character = PlatformerLevelManager.Instance.PlayerCharacter;
        WeaponBehaviour weaponBehaviour = character.FindBehaviour<WeaponBehaviour>();
        // _profile.data.weapons = weaponBehaviour.Inventory.ToArray()

        LogExt.Log<ProfileManager>($"Hit Checkpoint and saving profile. Level: {_profile.data.level}, Checkpoint: {_profile.data.checkpoint}");
        _profile.Save();
      }
    }

    private void OnEnable()
    {
      this.AddListener<CheckpointEvent>();
    }

    private void OnDisable()
    {
      this.RemoveListener<CheckpointEvent>();
    }

  }
}