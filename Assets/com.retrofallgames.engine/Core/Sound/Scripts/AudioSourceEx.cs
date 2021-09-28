using System.Collections;
using UnityEngine;

namespace RFG.Core
{
  public static class AudioSourceEx
  {
    public static void PlayAll(this AudioSource[] audioSources, float pitchMin = 0, float pitchMax = 0)
    {
      foreach (AudioSource source in audioSources)
      {
        if (!source.isPlaying)
        {
          if (pitchMin != 0 && pitchMax != 0)
          {
            source.pitch = UnityEngine.Random.Range(pitchMin, pitchMax);
          }
          source.Play();
        }
      }
    }

    public static IEnumerator FadeIn(this AudioSource audioSource, float duration)
    {
      float currentTime = 0;
      float start = 0;
      float volume = audioSource.volume;
      audioSource.Play();
      while (currentTime < duration)
      {
        currentTime += Time.unscaledDeltaTime;
        audioSource.volume = Mathf.Lerp(start, volume, currentTime / duration);
        yield return null;
      }
      yield break;
    }

    public static IEnumerator FadeOut(this AudioSource audioSource, float duration)
    {
      float currentTime = 0;
      float volume = audioSource.volume;
      float start = volume;
      while (currentTime < duration)
      {
        currentTime += Time.unscaledDeltaTime;
        audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
        yield return null;
      }
      audioSource.Stop();
      audioSource.volume = volume;
      yield break;
    }

  }
}