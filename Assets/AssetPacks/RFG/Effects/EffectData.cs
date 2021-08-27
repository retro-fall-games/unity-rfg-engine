using UnityEngine;

namespace RFG
{
  [CreateAssetMenu(fileName = "New Effect Data", menuName = "RFG/Effects/Effect Data")]
  public class EffectData : ScriptableObject
  {
    [Header("Settings")]
    public float Lifetime = 3f;
    public Vector3 Offset;
    public bool FlipY = false;

    [Header("Animations")]
    public string AnimationClip;
    [Header("Sound Effects")]
    public SoundData[] SoundEffects;
    [Header("Effects")]
    public string[] SpawnEffects;
    [Header("Camera Shake")]
    public float CameraShakeIntensity = 0f;
    public float CameraShakeTime = 0f;
    public bool CameraShakeFade = false;
  }
}