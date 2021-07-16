
using UnityEngine;
using System.Collections.Generic;

namespace RFGFx
{
  public class AmbienceAudio : MonoBehaviour
  {
    public static AmbienceAudio Instance { get; private set; }
    private Dictionary<string, AudioSource> ambience = new Dictionary<string, AudioSource>();
    private Dictionary<string, float> ambienceVolumes = new Dictionary<string, float>();

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;

        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
          ambience.Add(audioSource.clip.name, audioSource);
          ambienceVolumes.Add(audioSource.clip.name, audioSource.volume);
        }
      }
    }

    public void Play(string name, bool fade = false)
    {
      if (ambience.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeInVolume(ambience[name], ambienceVolumes[name], 1f));
        }
        else
        {
          ambience[name].volume = ambienceVolumes[name];
          ambience[name].Play();
        }
      }
    }

    public void Stop(string name, bool fade = false)
    {
      if (ambience.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeOutVolume(ambience[name], ambienceVolumes[name], 1f, true));
        }
        else
        {
          ambience[name].Stop();
        }
      }
    }

    public void StopAll(bool fade = false)
    {
      foreach (string key in ambience.Keys)
      {
        Stop(key, fade);
      }
    }

    public void SetVolume(float volume)
    {
      foreach (AudioSource audioSource in ambience.Values)
      {
        audioSource.volume = volume;
      }
    }

  }
}