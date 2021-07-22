
using UnityEngine;
using System.Collections.Generic;
using RFG;

namespace RFGFx
{
  public class SoundTrackAudio : Singleton<SoundTrackAudio>
  {
    public string playOnStart;
    public Dictionary<string, AudioSource> soundtrack = new Dictionary<string, AudioSource>();
    private Dictionary<string, float> soundtrackVolumes = new Dictionary<string, float>();

    protected override void Awake()
    {
      base.Awake();

      AudioSource[] audioSources = GetComponents<AudioSource>();

      foreach (AudioSource audioSource in audioSources)
      {
        soundtrack.Add(audioSource.clip.name, audioSource);
        soundtrackVolumes.Add(audioSource.clip.name, audioSource.volume);
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

    public void Play(string[] names, bool fade = false)
    {
      foreach (string name in names)
      {
        Play(name, fade);
      }
    }

    public void Stop(string name, bool fade = false)
    {
      if (soundtrack.ContainsKey(name))
      {
        if (fade)
        {
          StartCoroutine(VolumnFade.FadeOutVolume(soundtrack[name], soundtrackVolumes[name], 1f));
        }
        else
        {
          soundtrack[name].Stop();
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