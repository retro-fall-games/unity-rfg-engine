using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/Level Bounds")]
    public class LevelBoundsBehaviour : MonoBehaviour
    {
      [Header("Settings")]
      [Tooltip("The level bounds to constrain the character")]
      public LevelBounds LevelBounds;

      [HideInInspector]
      private Character _character;

      private void Awake()
      {
        _character = GetComponent<Character>();
      }

      private void LateUpdate()
      {
        if (LevelBounds != null)
        {
          LevelBounds.HandleLevelBounds(_character);
        }
      }
    }
  }
}