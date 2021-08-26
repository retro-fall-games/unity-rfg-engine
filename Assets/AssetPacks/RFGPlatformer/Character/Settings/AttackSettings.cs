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

      [Header("Effects")]
      public string[] AttackEffects;

      [Header("Animations")]
      [Tooltip("Define what layer to play animations")]
      public string Layer = "Base Layer";

      [Tooltip("Define what animation to play for attacking")]
      public string AttackClip;

    }
  }
}