using System.Collections;

namespace RFG
{
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    public int loadProfileId = -1;
    private Profile _profile;

    private void Start()
    {
      if (loadProfileId > -1)
      {
        _profile = new Profile();
        _profile.Load(loadProfileId);
      }
    }
    public void SetProfile(Profile profile)
    {
      _profile = profile;
    }

    public Profile GetProfile()
    {
      return _profile;
    }

    public void SaveProfile()
    {
      StartCoroutine(SaveProfileCo());
    }

    private IEnumerator SaveProfileCo()
    {
      // _profile.xp = 0;
      // _profile.timePlayed = 0;
      _profile.Save();
      yield break;
    }

  }
}