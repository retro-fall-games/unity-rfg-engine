using System.Collections;
using UnityEngine;

namespace RFG
{
  public static class AudioSourceEx
  {
    public static void PlayAll(this AudioSource[] audioSources)
    {
      foreach (AudioSource source in audioSources)
      {
        source.Play();
      }
    }

    public static IEnumerator FadeIn(this AudioSource audioSource, float targetVolume, float duration)
    {
      float currentTime = 0;
      float start = 0;
      audioSource.Play();
      while (currentTime < duration)
      {
        currentTime += Time.unscaledDeltaTime;
        audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
        yield return null;
      }
      yield break;
    }
    public static IEnumerator FadeOut(this AudioSource audioSource, float targetVolume, float duration)
    {
      float currentTime = 0;
      float start = targetVolume;
      while (currentTime < duration)
      {
        currentTime += Time.unscaledDeltaTime;
        audioSource.volume = Mathf.Lerp(start, 0, currentTime / duration);
        yield return null;
      }
      audioSource.Stop();
      yield break;
    }

  }
}