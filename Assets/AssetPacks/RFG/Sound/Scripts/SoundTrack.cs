using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG/Sound/Sound Track")]
  public class SoundTrack : SoundBase<SoundTrack>
  {
    protected override void Awake()
    {
      VolumeName = "SoundTrackVolume";
      base.Awake();
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void AddAudioSource()
    {
      GameObject soundTrack = new GameObject("SoundTrack");
      soundTrack.AddComponent<AudioSource>();
      soundTrack.tag = "SoundTrack";
      soundTrack.gameObject.transform.SetParent(transform);
    }
#endif
  }
}