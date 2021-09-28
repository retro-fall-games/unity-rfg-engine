
using UnityEngine;
using System.Collections.Generic;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Sound/Sound Base")]
  public class SoundBase : MonoBehaviour
  {
    [Header("Sound Base")]
    public SoundBaseSettings Settings;
    public bool Loaded { get; private set; }

    [HideInInspector]
    protected Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

    private void Awake()
    {
      Loaded = false;
      // Get all the audio source and make a dictionary to be able to reference by name
      AudioSource[] audioSourceComponents = GetComponentsInChildren<AudioSource>();
      foreach (AudioSource audioSource in audioSourceComponents)
      {
        _audioSources.Add(audioSource.clip.name, audioSource);
      }
    }

    private void Start()
    {
      Settings.SetVolume(Settings.GetPlayerPrefsVolume());
      Loaded = true;
    }

    public AudioSource GetAudioSource(string name)
    {
      if (_audioSources.ContainsKey(name))
      {
        return _audioSources[name];
      }
      return null;
    }

    public void Play(string name)
    {
      if (_audioSources.ContainsKey(name))
      {
        _audioSources[name].Play();
      }
    }

    public void Play(string name, bool fade = false)
    {
      if (_audioSources.ContainsKey(name))
      {
        AudioSource audio = _audioSources[name];
        if (fade)
        {
          StartCoroutine(audio.FadeIn(Settings.FadeTime));
        }
        else
        {
          audio.Play();
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
      if (_audioSources.ContainsKey(name))
      {
        AudioSource audio = _audioSources[name];
        if (fade)
        {
          StartCoroutine(audio.FadeOut(Settings.FadeTime));
        }
        else
        {
          audio.Stop();
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
      foreach (string key in _audioSources.Keys)
      {
        Stop(key, fade);
      }
    }

    public bool IsPlaying(string name)
    {
      if (_audioSources.ContainsKey(name))
      {
        return _audioSources[name].isPlaying;
      }
      return false;
    }

#if UNITY_EDITOR
    public void ConfigureAudioSources()
    {
      Sound[] sounds = GetComponentsInChildren<Sound>();
      foreach (Sound sound in sounds)
      {
        sound.GenerateAudioData();
      }
    }
#endif

  }
}