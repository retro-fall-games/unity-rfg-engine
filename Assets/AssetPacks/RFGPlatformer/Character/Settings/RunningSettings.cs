using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Running Settings", menuName = "RFG/Platformer/Character/Settings/Running")]
    public class RunningSettings : ScriptableObject
    {
      [Header("Settings")]
      public float RunningSpeed = 5f;
      public float RunningPower = 5f;
      public float PowerGainPerFrame = .01f;
      public float CooldownTimer = 5f;
    }
  }
}