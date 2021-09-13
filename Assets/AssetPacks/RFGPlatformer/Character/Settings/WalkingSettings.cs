using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Walking Settings", menuName = "RFG/Platformer/Character/Settings/Walking")]
    public class WalkingSettings : ScriptableObject
    {
      [Header("Settings")]
      public float WalkingSpeed = 5f;
    }
  }
}