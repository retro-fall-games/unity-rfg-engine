using UnityEngine;

namespace RFG.Utils
{
  public static class Physics2D
  {
    public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
    {
      Debug.DrawRay(origin, direction * distance, Color.red);
      return UnityEngine.Physics2D.Raycast(origin, direction, distance, layerMask);
    }
  }
}