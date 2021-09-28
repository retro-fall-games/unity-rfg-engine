using UnityEngine;
using UnityEngine.Audio;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Sound Data", menuName = "RFG/Core/Sound/Sound Data")]
  public class SoundData : ScriptableObject
  {
    public enum SoundType { SoundTrack, SoundAmbience, SoundFx };
    public AudioClip clip;
    public SoundType type;
    public AudioMixerGroup output;

    [Range(0, 1)]
    public float Volume = 1f;

    [Range(.25f, 3)]
    public float Pitch = 1f;
    public bool PlayOnAwake = false;

    public bool Loop = false;

    [Range(0f, 1f)]
    public float SpacialBlend = 1f;

    public float MinDistance = 1f;
    public float MaxDistance = 100f;
    public AudioRolloffMode RolloffMode;
  }

  public static class SoundDataEx
  {
    public static void GenerateAudioSource(this SoundData soundData, GameObject gameObject)
    {
      AudioSource source = gameObject.GetComponent<AudioSource>();
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
}