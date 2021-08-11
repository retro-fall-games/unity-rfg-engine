using UnityEngine;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Effect Data", menuName = "RFG/Effects/Effect Data")]
  public class EffectData : ScriptableObject
  {
    public float Lifetime = 3f;
    public SoundData[] SoundFx;
    public string[] ObjectsToSpawn;

  }
}