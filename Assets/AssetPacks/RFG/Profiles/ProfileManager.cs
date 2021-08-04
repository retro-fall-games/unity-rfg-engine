using System.Collections;

namespace RFG
{
  public class ProfileManager : PersistentSingleton<ProfileManager>
  {
    public int loadProfileId = -1;
    private Profile<ProfileData> _profile;

    private void Start()
    {
      if (loadProfileId > -1)
      {
        _profile = new Profile<ProfileData>();
        _profile.Load(loadProfileId);
      }
    }
    public void SetProfile(Profile<ProfileData> profile)
    {
      _profile = profile;
    }

    public Profile<ProfileData> GetProfile()
    {
      return _profile;
    }

    public void SaveProfile()
    {
      StartCoroutine(SaveProfileCo());
    }

    private IEnumerator SaveProfileCo()
    {
      _profile.Save();
      yield break;
    }

  }
}