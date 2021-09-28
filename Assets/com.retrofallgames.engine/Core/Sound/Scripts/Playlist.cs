using UnityEngine;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Playlist", menuName = "RFG/Core/Sound/Playlist")]
  public class Playlist : ScriptableObject
  {
    [Header("Playlist")]
    public SoundData[] Sounds;
    public bool Loop = true;
    public float WaitForSeconds = 1f;
    public float FadeTime = 1f;
    public int CurrentIndex = 0;
  }
}