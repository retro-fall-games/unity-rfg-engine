using RFG;

namespace Game
{
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    public int loadProfileId = -1;
    public Profile Profile
    {
      get
      {
        return _profile;
      }
      set
      {
        _profile = value;
      }
    }
    private Profile _profile;

    protected override void Awake()
    {
      base.Awake();
      // This is here for debugging purposes, this wont be used in a real game
      // if (loadProfileId > -1)
      // {
      //   // LogExt.Log<ProfileManager>("Loading profile: " + loadProfileId);
      //   _profile = new Profile<Game.ProfileData>();
      //   _profile.Load(loadProfileId);
      //   if (_profile.data != null)
      //   {
      //     PlayerPrefs.SetInt("startingCheckpoint", _profile.data.checkpoint);
      //   }
      //   else
      //   {
      //     LogExt.Warn<ProfileManager>("Did not load profile");
      //   }
      // }
    }

  }
}