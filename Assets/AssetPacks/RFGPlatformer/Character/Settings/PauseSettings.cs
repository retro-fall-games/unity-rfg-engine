using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Pause Settings", menuName = "RFG/Platformer/Character/Settings/Pause")]
    public class PauseSettings : ScriptableObject
    {
      [Header("Game Events")]
      public GameEvent PauseEvent;

      [Header("Effects")]
      public string[] PauseEffects;
      public string[] UnPauseEffects;

    }
  }
}