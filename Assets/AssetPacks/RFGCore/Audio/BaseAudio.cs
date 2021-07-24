
using UnityEngine;
using System.Collections.Generic;

namespace RFG
{
  public class BaseAudio<T> : Singleton<T> where T : Component
  {
    public float Volume { get; set; }
    public Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    protected string VolumeName { get; set; }
    private Dictionary<string, float> audioSourceVolumes = new Dictionary<string, float>();

    protected override void Awake()
    {
      base.Awake();

      AudioSource[] audioSourceComponents = GetComponents<AudioSource>();

      foreach (AudioSource audioSource in audioSourceComponents)
      {
        audioSources.Add(audioSource.clip.name, audioSource);
        audioSourceVolumes.Add(audioSource.clip.name, audioSource.volume);
        audioSource.volume = 0;
      }

      // Set the volume for every audio source
      float volume = PlayerPrefs.GetFloat(VolumeName, 1);
      SetVolume(volume);
    }

    public void Play(string name, bool fade = false)
    {
      if (audioSources.ContainsKey(name))
      {
        if (fade)
        {
          float volume = audioSourceVolumes[name];
          if (volume > Volume)
          {
            volume = Volume;
          }
          StartCoroutine(VolumnFade.FadeInVolume(audioSources[name], volume, 1f));
        }
        else
        {
          audioSources[name].Play();
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
      if (audioSources.ContainsKey(name))
      {
        if (fade)
        {
          float volume = audioSourceVolumes[name];
          if (volume > Volume)
          {
            volume = Volume;
          }
          StartCoroutine(VolumnFade.FadeOutVolume(audioSources[name], volume, 1f));
        }
        else
        {
          audioSources[name].Stop();
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
      foreach (string key in audioSources.Keys)
      {
        Stop(key, fade);
      }
    }

    public void SetVolume(float volume)
    {
      // Keep a copy in the class
      Volume = volume;

      // Store the value in player prefs
      PlayerPrefs.SetFloat(VolumeName, volume);

      // Set the volume for each audio source in the dictionary
      foreach (AudioSource audioSource in audioSources.Values)
      {
        float originalVolume = audioSourceVolumes[audioSource.clip.name];
        if (volume > originalVolume)
        {
          volume = originalVolume;
        }
        audioSource.volume = volume;
      }
    }

  }
}