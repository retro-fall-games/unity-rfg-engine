using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    public class LevelBoundsResizer : MonoBehaviour
    {
      public LevelBounds LevelBounds;

#if UNITY_EDITOR
      [ButtonMethod]
      private void ResizeToLevelBounds()
      {
        transform.localScale = LevelBounds.Bounds.size;
        transform.position = LevelBounds.Bounds.center;
      }
#endif
    }
  }
}