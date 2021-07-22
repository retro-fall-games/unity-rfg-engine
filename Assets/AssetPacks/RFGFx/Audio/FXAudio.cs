
using UnityEngine;
using System.Collections.Generic;
using RFG;

namespace RFGFx
{
  public class FXAudio : Singleton<FXAudio>
  {
    private Dictionary<string, AudioSource> fx = new Dictionary<string, AudioSource>();
    private Dictionary<string, float> fxVolumes = new Dictionary<string, float>();

    protected override void Awake()
    {
      base.Awake();

      AudioSource[] audioSources = GetComponents<AudioSource>();

      foreach (AudioSource audioSource in audioSources)
      {
        fx.Add(audioSource.clip.name, audioSource);
        fxVolumes.Add(audioSource.clip.name, audioSource.volume);
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

    public void Play(string[] names, bool fade = false)
    {
      foreach (string name in names)
      {
        Play(name, fade);
      }
    }

    public void Stop(string name, bool fade = false)
    {
      if (fx.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeOutVolume(fx[name], fxVolumes[name], 1f));
        }
        else
        {
          fx[name].Stop();
        }
      }
    }

    public void Stop(string[] names, bool fade = false)
    {
      foreach (string name in names)
      {
        Stop(name, fade);
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