
using UnityEngine;
using System;
using System.Collections.Generic;

namespace RFGFx
{
  public class SoundTrackAudio : MonoBehaviour
  {
    public static SoundTrackAudio Instance { get; private set; }
    public string playOnStart;
    public Dictionary<string, AudioSource> soundtrack = new Dictionary<string, AudioSource>();
    private Dictionary<string, float> soundtrackVolumes = new Dictionary<string, float>();

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;

        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
          soundtrack.Add(audioSource.clip.name, audioSource);
          soundtrackVolumes.Add(audioSource.clip.name, audioSource.volume);
        }
      }
    }

    private void Start()
    {
      if (playOnStart != null)
      {
        Play(playOnStart);
      }
    }

    public void Play(string name, bool fade = false)
    {
      if (soundtrack.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeInVolume(soundtrack[name], soundtrackVolumes[name], 1f));
        }
        else
        {
          soundtrack[name].volume = soundtrackVolumes[name];
          soundtrack[name].Play();
        }
      }
    }

    public void Stop(string name, bool fade = false)
    {
      if (soundtrack.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeOutVolume(soundtrack[name], soundtrackVolumes[name], 1f, true));
        }
        else
        {
          soundtrack[name].Stop();
        }
      }
    }

    public void StopAll(bool fade = false)
    {
      foreach (string key in soundtrack.Keys)
      {
        Stop(key, fade);
      }
    }

    public void SetVolume(float volume)
    {
      foreach (AudioSource audioSource in soundtrack.Values)
      {
        audioSource.volume = volume;
      }
    }

  }
}