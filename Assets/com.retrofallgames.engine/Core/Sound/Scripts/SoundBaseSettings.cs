using UnityEngine;
using UnityEngine.Audio;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Sound Base Settings", menuName = "RFG/Core/Sound/Sound Base Settings")]
  public class SoundBaseSettings : ScriptableObject
  {
    public string Name;
    public AudioMixer AudioMixer;
    public float FadeTime = 1f;
    public float Volume { get; set; }

    public void Awake()
    {
      SetVolume(GetPlayerPrefsVolume());
    }

    public float GetPlayerPrefsVolume()
    {
      return PlayerPrefs.GetFloat($"Volume{Name}", 1);
    }

    public void SetVolume(float volume)
    {
      if (volume < 0.001f)
      {
        volume = 0.001f;
      }

      // Keep a copy in the class
      Volume = volume;

      // Store the value in player prefs
      PlayerPrefs.SetFloat($"Volume{Name}", volume);

      // Set the volume on the mixer
      AudioMixer.SetFloat("Volume", Mathf.Log(volume) * 20);
    }
  }
}