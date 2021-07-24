using System.Collections;
using UnityEngine;

namespace RFG
{
  public class VolumnFade : MonoBehaviour
  {
    public static IEnumerator FadeInVolume(AudioSource audioSource, float targetVolume, float duration)
    {
      float currentTime = 0;
      float start = 0;
      while (currentTime < duration)
      {
        currentTime += Time.unscaledDeltaTime;
        audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
        yield return null;
      }
      audioSource.Play();
      yield break;
    }
    public static IEnumerator FadeOutVolume(AudioSource audioSource, float targetVolume, float duration)
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