using UnityEngine;
using MyBox;

namespace RFG
{
  [AddComponentMenu("RFG/Sound/Sound FX")]
  public class SoundFx : SoundBase<SoundFx>
  {
    protected override void Awake()
    {
      VolumeName = "SoundFxVolume";
      base.Awake();
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void AddAudioSource()
    {
      GameObject soundFx = new GameObject("SoundFx");
      soundFx.AddComponent<AudioSource>();
      soundFx.tag = "SoundFx";
      soundFx.gameObject.transform.SetParent(transform);
    }
#endif

  }
}