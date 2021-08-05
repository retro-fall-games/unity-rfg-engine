using UnityEngine;
using UnityEngine.Audio;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Sound Data", menuName = "RFG/Sound/Sound Data")]
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
}