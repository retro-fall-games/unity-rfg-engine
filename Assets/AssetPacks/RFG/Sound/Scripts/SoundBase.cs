
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RFG
{
  public class SoundBase : MonoBehaviour
  {
    [Header("Sound Base")]
    public SoundBaseSettings Settings;

    [Header("Play On Awake")]
    public Sound[] soundtrack;
    public bool loopSoundtrack = true;
    public float soundtrackWaitForSeconds = 1f;

    [HideInInspector]
    protected Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    private Sound _currentPlayingSoundtrack;
    private int _currentPlayingSoundtrackIndex = 0;

    private void Awake()
    {
      // Get all the audio source and make a dictionary to be able to reference by name
      AudioSource[] audioSourceComponents = GetComponentsInChildren<AudioSource>();
      foreach (AudioSource audioSource in audioSourceComponents)
      {
        audioSources.Add(audioSource.clip.name, audioSource);
      }
    }

    private void Start()
    {
      if (soundtrack.Length > 0)
      {
        StartCoroutine(PlaySoundTrack());
      }
    }

    private IEnumerator PlaySoundTrack()
    {
      AudioSource audio = soundtrack[_currentPlayingSoundtrackIndex].GetComponent<AudioSource>();
      while (true)
      {
        if (!audio.isPlaying)
        {
          audio = soundtrack[_currentPlayingSoundtrackIndex].GetComponent<AudioSource>();
          audio.Play();
          if (++_currentPlayingSoundtrackIndex == soundtrack.Length)
          {
            _currentPlayingSoundtrackIndex = 0;
          }
        }
        yield return null;
      }
    }

    public void Play(string name)
    {
      if (audioSources.ContainsKey(name))
      {
        audioSources[name].Play();
      }
    }

    public void Play(string name, bool fade = false)
    {
      if (audioSources.ContainsKey(name))
      {
        AudioSource audio = audioSources[name];
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
      if (audioSources.ContainsKey(name))
      {
        AudioSource audio = audioSources[name];
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
      foreach (string key in audioSources.Keys)
      {
        Stop(key, fade);
      }
    }

    public bool IsPlaying(string name)
    {
      if (audioSources.ContainsKey(name))
      {
        return audioSources[name].isPlaying;
      }
      return false;
    }

    public void ConfigureAudioSources()
    {
      Sound[] sounds = GetComponentsInChildren<Sound>();
      foreach (Sound sound in sounds)
      {
        sound.GenerateAudioData();
      }
    }

  }
}