using UnityEngine;

namespace RFG
{
  public static class Physics2D
  {
    public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask, Color color)
    {
      if (GameManager.Instance != null && GameManager.Instance.Settings.DrawRaycasts)
      {
        Debug.DrawRay(origin, direction * distance, color == null ? Color.red : color);
      }
      return UnityEngine.Physics2D.Raycast(origin, direction, distance, layerMask);
    }
  }
}