using UnityEngine;
using MyBox;

namespace RFG
{
  public class Sound : MonoBehaviour
  {
    public SoundData soundData;

#if UNITY_EDITOR
    [ButtonMethod]
    public void GenerateAudioData()
    {
      if (soundData != null)
      {
        gameObject.name = soundData.clip.name;
        AudioSource source = GetComponent<AudioSource>();
        if (source == null)
        {
          source = gameObject.AddComponent<AudioSource>();
        }
        source.tag = soundData.type.ToString();
        source.clip = soundData.clip;
        source.outputAudioMixerGroup = soundData.output;
        source.playOnAwake = soundData.PlayOnAwake;
        source.loop = soundData.Loop;
        source.volume = soundData.Volume;
        source.spatialBlend = soundData.SpacialBlend;
        source.pitch = soundData.Pitch;
        source.minDistance = soundData.MinDistance;
        source.maxDistance = soundData.MaxDistance;
        source.rolloffMode = soundData.RolloffMode;
      }
    }
#endif

  }
}