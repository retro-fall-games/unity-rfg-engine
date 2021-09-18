using System;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace SceneGraph
  {
    public class DoorConnection
    {
      public Door inPoint;
      public Door outPoint;
      public Action<DoorConnection> OnClickRemoveConnection;

      public DoorConnection(Door inPoint, Door outPoint, Action<DoorConnection> OnClickRemoveConnection)
      {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
      }

      public void Draw()
      {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center - Vector2.left * 50f,
            Color.white,
            null,
            2f
        );

        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
        {
          if (OnClickRemoveConnection != null)
          {
            OnClickRemoveConnection(this);
          }
        }
      }
    }
  }
}