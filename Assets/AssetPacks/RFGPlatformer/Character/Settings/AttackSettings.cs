using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Attack Settings", menuName = "RFG/Platformer/Character/Settings/Attack")]
    public class AttackSettings : ScriptableObject
    {
      [Header("Settings")]
      public float AttackSpeed = 5f;
    }
  }
}