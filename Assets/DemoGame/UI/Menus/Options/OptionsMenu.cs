using UnityEngine;
using UnityEngine.UI;
using RFG.Core;
using RFG.SceneGraph;

namespace Game
{
  public class OptionsMenu : MonoBehaviour
  {
    [Header("Sound Base Settings")]
    public SoundBaseSettings SoundTrack;
    public SoundBaseSettings SoundAmbience;
    public SoundBaseSettings SoundFx;

    [Header("UI")]
    public Slider soundTrackVolumeSlider;
    public Slider fxVolumeSlider;
    public Slider ambienceVolumeSlider;

    private void Start()
    {
      soundTrackVolumeSlider.value = SoundTrack.Volume;
      fxVolumeSlider.value = SoundFx.Volume;
      ambienceVolumeSlider.value = SoundAmbience.Volume;
    }

    public void QuitToTitle()
    {
      SceneManager.Instance.LoadScene("Title");
    }

  }
}