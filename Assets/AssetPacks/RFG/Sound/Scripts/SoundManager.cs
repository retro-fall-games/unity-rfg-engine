using UnityEngine;
namespace RFG
{
  [AddComponentMenu("RFG/Sound/Sound Manager")]
  public class SoundManager : PersistentSingleton<SoundManager>
  {
    public void StopAll(bool fade = false)
    {
      // SoundTrack.Instance.StopAll(fade);
      // SoundAmbience.Instance.StopAll(fade);
      // SoundFx.Instance.StopAll(fade);
    }

  }
}