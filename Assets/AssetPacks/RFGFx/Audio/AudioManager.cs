using System.Collections;
using UnityEngine;

namespace RFGFx
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
      if (Instance != null && Instance != this)
        Destroy(gameObject);
      else
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
    }

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