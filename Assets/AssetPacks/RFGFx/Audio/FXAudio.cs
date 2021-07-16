
using UnityEngine;
using System.Collections.Generic;

namespace RFGFx
{
  public class FXAudio : MonoBehaviour
  {
    public static FXAudio Instance { get; private set; }
    private Dictionary<string, AudioSource> fx = new Dictionary<string, AudioSource>();
    private Dictionary<string, float> fxVolumes = new Dictionary<string, float>();

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;

        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
          fx.Add(audioSource.clip.name, audioSource);
          fxVolumes.Add(audioSource.clip.name, audioSource.volume);
        }
      }
    }

    public void Play(string name, bool fade = false)
    {
      if (fx.ContainsKey(name))
      {

        if (fade)
        {
          StartCoroutine(VolumnFade.FadeInVolume(fx[name], fxVolumes[name], 1f));
        }
        else
        {
          fx[name].volume = fxVolumes[name];
          fx[name].Play();
        }
      }
    }

    public void Stop(string name, bool fade = false)
    {
      if (fx.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeOutVolume(fx[name], fxVolumes[name], 1f, true));
        }
        else
        {
          fx[name].Stop();
        }
      }
    }

    public void StopAll(bool fade = false)
    {
      foreach (string key in fx.Keys)
      {
        Stop(key, fade);
      }
    }

    public void SetVolume(float volume)
    {
      foreach (AudioSource audioSource in fx.Values)
      {
        audioSource.volume = volume;
      }
    }

  }
}