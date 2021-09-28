using UnityEngine;

namespace RFG.Effects
{
  using Core;

  [CreateAssetMenu(fileName = "New Effect Data", menuName = "RFG/Effects/Effect Data")]
  public class EffectData : ScriptableObject
  {
    [Header("Settings")]
    public float Lifetime = 3f;
    public bool PooledObject = true;

    [Header("Animations")]
    public string AnimationClip;
    [Header("Sound Effects")]
    public SoundData[] SoundEffects;
    [Range(0.0f, 1.0f)]
    public float PitchMin;
    [Range(0.0f, 1.0f)]
    public float PitchMax;
    [Header("Effects")]
    public string[] SpawnEffects;
    [Header("Camera Shake")]
    public float CameraShakeIntensity = 0f;
    public float CameraShakeTime = 0f;
    public bool CameraShakeFade = false;
  }
}