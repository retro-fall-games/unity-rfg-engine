
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace RFG
{
  public class SoundBase<T> : Singleton<T> where T : Component
  {
    [Header("Settings")]
    public AudioMixer AudioMixer;
    public float FadeTime = 1f;
    public float Volume { get; set; }

    [HideInInspector]
    protected Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    protected string VolumeName { get; set; }

    protected override void Awake()
    {
      base.Awake();

      // Get all the audio source and make a dictionary to be able to reference by name
      AudioSource[] audioSourceComponents = GetComponentsInChildren<AudioSource>();
      foreach (AudioSource audioSource in audioSourceComponents)
      {
        audioSources.Add(audioSource.clip.name, audioSource);
      }

      // Set the volume
      float volume = PlayerPrefs.GetFloat(VolumeName, 1);
      SetVolume(volume);
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
          StartCoroutine(audio.FadeIn(FadeTime));
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
          StartCoroutine(audio.FadeOut(FadeTime));
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

    public void SetVolume(float volume)
    {
      if (volume < 0.001f)
      {
        volume = 0.001f;
      }

      // Keep a copy in the class
      Volume = volume;

      // Store the value in player prefs
      PlayerPrefs.SetFloat(VolumeName, volume);

      // Set the volume on the mixer
      AudioMixer.SetFloat("Volume", Mathf.Log(volume) * 20);
    }

    public bool IsPlaying(string name)
    {
      if (audioSources.ContainsKey(name))
      {
        return audioSources[name].isPlaying;
      }
      return false;
    }

  }
}