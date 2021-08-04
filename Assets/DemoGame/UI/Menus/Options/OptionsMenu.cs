using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using RFG;

namespace Game
{
  public class OptionsMenu : MonoBehaviour
  {

    public Slider soundTrackVolumeSlider;
    public Slider fxVolumeSlider;
    public Slider ambienceVolumeSlider;
    // private void Start()
    // {
    //   StartCoroutine(StartCo());
    // }

    // private IEnumerator StartCo()
    // {
    //   // yield return new WaitUntil(() => SoundTrackAudio.Instance != null);
    //   // // yield return new WaitUntil(() => FXAudio.Instance != null);
    //   // yield return new WaitUntil(() => AmbienceAudio.Instance != null);
    //   // if (soundTrackVolumeSlider != null)
    //   // {
    //   //   soundTrackVolumeSlider.value = SoundTrackAudio.Instance.Volume;
    //   // }
    //   // // if (fxVolumeSlider != null)
    //   // // {
    //   // //   fxVolumeSlider.value = FXAudio.Instance.Volume;
    //   // // }
    //   // if (ambienceVolumeSlider != null)
    //   // {
    //   //   ambienceVolumeSlider.value = AmbienceAudio.Instance.Volume;
    //   // }
    // }

    public void SetSoundtrackVolume(float volume)
    {
      // if (SoundTrackAudio.Instance != null)
      // {
      //   SoundTrackAudio.Instance.SetVolume(volume);
      // }
    }

    public void SetFXVolume(float volume)
    {
      // if (FXAudio.Instance != null)
      // {
      //   FXAudio.Instance.SetVolume(volume);
      // }
    }

    // public void SetAmbienceVolume(float volume)
    // {
    //   if (AmbienceAudio.Instance != null)
    //   {
    //     AmbienceAudio.Instance.SetVolume(volume);
    //   }
    // }

  }
}