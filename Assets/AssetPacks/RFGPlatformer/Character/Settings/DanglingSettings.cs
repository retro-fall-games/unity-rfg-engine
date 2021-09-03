using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [CreateAssetMenu(fileName = "New Dangling Settings", menuName = "RFG/Platformer/Character/Settings/Dangling")]
    public class DanglingSettings : ScriptableObject
    {
      [Header("Settings")]
      /// the origin of the raycast used to detect pits. This is relative to the transform.position of our character
      [Tooltip("the origin of the raycast used to detect pits. This is relative to the transform.position of our character")]
      public Vector3 DanglingRaycastOrigin = new Vector3(0.7f, -0.25f, 0f);

      /// the length of the raycast used to detect pits
      [Tooltip("the length of the raycast used to detect pits")]
      public float DanglingRaycastLength = 2f;

      [Header("Animations")]

      [Tooltip("Define what animation to play for dangling")]
      public string DanglingClip;

      [Header("Effects")]
      public string[] DanglingEffects;

    }
  }
}