using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dash Settings", menuName = "RFG/Platformer/Character/Settings/Dash")]
    public class DashSettings : ScriptableObject
    {
      [Header("Dash")]
      public float DashDistance = 3f;
      public float DashForce = 40f;
      public int TotalDashes = 2;
      public float MinInputThreshold = 0.1f;

      [Header("Cooldown")]
      public float Cooldown = 1f;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for dashing")]
      public string DashingClip;

      [Header("Effects")]
      public string[] DashEffects;
    }
  }
}