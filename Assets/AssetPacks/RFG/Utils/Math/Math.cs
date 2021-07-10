using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG.Utils
{
  public static class Math
  {
    public static float DistanceBetweenPointAndLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
      return Vector3.Magnitude(ProjectPointOnLine(point, lineStart, lineEnd) - point);
    }

    public static Vector3 ProjectPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
    {
      Vector3 rhs = point - lineStart;
      Vector3 vector = lineEnd - lineStart;
      float magnitude = vector.magnitude;
      Vector3 lhs = vector;
      if (magnitude > 1E-06f)
      {
        lhs = (Vector3)(lhs / magnitude);
      }
      float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
      return (lineStart + (Vector3)(lhs * num2));
    }
  }
}