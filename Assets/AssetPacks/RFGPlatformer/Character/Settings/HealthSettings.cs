using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Health Settings", menuName = "RFG/Platformer/Character/Settings/Health")]
    public class HealthSettings : ScriptableObject
    {
      public float Health = 100f;
      public float MaxHealth = 100f;
      public float DefaultMaxHealth = 100f;
    }
  }
}