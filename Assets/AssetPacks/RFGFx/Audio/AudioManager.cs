using System.Collections;
using UnityEngine;
using RFG;

namespace RFGFx
{
  public class AudioManager : PersistentSingleton<AudioManager>
  {

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => SoundTrackAudio.Instance != null);
      yield return new WaitUntil(() => AmbienceAudio.Instance != null);
      yield return new WaitUntil(() => FXAudio.Instance != null);
      if (PlayerPrefs.HasKey("soundTrackVolume"))
      {
        SetSoundTrackVolume(PlayerPrefs.GetFloat("soundTrackVolume"));
      }
      if (PlayerPrefs.HasKey("ambienceVolume"))
      {
        SetAmbienceVolume(PlayerPrefs.GetFloat("ambienceVolume"));
      }
      if (PlayerPrefs.HasKey("fxVolume"))
      {
        SetFxVolume(PlayerPrefs.GetFloat("fxVolume"));
      }
    }

    public void SetSoundTrackVolume(float volume)
    {
      if (SoundTrackAudio.Instance != null)
      {
        PlayerPrefs.SetFloat("soundTrackVolume", volume);
        SoundTrackAudio.Instance.SetVolume(volume);
      }
    }
    public void SetAmbienceVolume(float volume)
    {
      if (AmbienceAudio.Instance != null)
      {
        PlayerPrefs.SetFloat("ambienceVolume", volume);
        AmbienceAudio.Instance.SetVolume(volume);
      }
    }
    public void SetFxVolume(float volume)
    {
      if (FXAudio.Instance != null)
      {
        PlayerPrefs.SetFloat("fxVolume", volume);
        FXAudio.Instance.SetVolume(volume);
      }
    }

    public void StopAll(bool fade = false)
    {
      FXAudio.Instance.StopAll(fade);
      SoundTrackAudio.Instance.StopAll(fade);
      AmbienceAudio.Instance.StopAll(fade);
    }

  }
}