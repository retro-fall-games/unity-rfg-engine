using UnityEngine;
using MyBox;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Sound/Sound")]
  public class Sound : MonoBehaviour
  {
    public SoundData soundData;

#if UNITY_EDITOR
    [ButtonMethod]
    public void GenerateAudioData()
    {
      if (soundData != null)
      {
        gameObject.name = soundData.clip.name;
        soundData.GenerateAudioSource(gameObject);
      }
    }
#endif

  }
}