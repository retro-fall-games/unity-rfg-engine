using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Idle Settings", menuName = "RFG/Platformer/Character/Settings/Idle")]
    public class IdleSettings : ScriptableObject
    {
      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for idle")]
      public string IdleClip;
    }
  }
}