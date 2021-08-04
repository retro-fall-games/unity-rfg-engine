using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG/Sound/Sound Ambience")]
  public class SoundAmbience : SoundBase<SoundAmbience>
  {
    protected override void Awake()
    {
      VolumeName = "SoundAmbienceVolume";
      base.Awake();
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void AddAudioSource()
    {
      GameObject soundAmbience = new GameObject("SoundAmbience");
      AudioSource audio = soundAmbience.AddComponent<AudioSource>();
      soundAmbience.tag = "SoundAmbience";
      soundAmbience.gameObject.transform.SetParent(transform);
    }
#endif
  }
}