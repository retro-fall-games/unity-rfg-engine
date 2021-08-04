using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG/Sound/Sound Manager")]
  public class SoundManager : PersistentSingleton<SoundManager>
  {
    public void StopAll(bool fade = false)
    {
      SoundTrack.Instance.StopAll(fade);
      SoundAmbience.Instance.StopAll(fade);
      SoundFx.Instance.StopAll(fade);
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void AddSubManagers()
    {
      if (transform.Find("SoundTrack") == null)
      {
        GameObject soundTrack = new GameObject("SoundTrack");
        soundTrack.AddComponent<SoundTrack>();
        soundTrack.gameObject.transform.SetParent(transform);
      }
      if (transform.Find("SoundAmbience") == null)
      {
        GameObject soundAmbience = new GameObject("SoundAmbience");
        soundAmbience.AddComponent<SoundAmbience>();
        soundAmbience.gameObject.transform.SetParent(transform);
      }
      if (transform.Find("SoundFx") == null)
      {
        GameObject soundFx = new GameObject("SoundFx");
        soundFx.AddComponent<SoundFx>();
        soundFx.gameObject.transform.SetParent(transform);
      }
    }
#endif
  }
}