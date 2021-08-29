using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Level/Level Bounds")]
    public class LevelBounds : MonoBehaviour
    {
      public enum BoundsBehaviour { Nothing, Constrain, Kill }

      [Header("Characters")]
      public List<Character> Characters;

      [Header("Level Bounds")]
      public BoundsBehaviour Top = BoundsBehaviour.Constrain;
      public BoundsBehaviour Bottom = BoundsBehaviour.Kill;
      public BoundsBehaviour Left = BoundsBehaviour.Constrain;
      public BoundsBehaviour Right = BoundsBehaviour.Constrain;
      public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one * 10);

      [HideInInspector]
      private Vector2 _constrainedPosition = Vector2.zero;

      private void LateUpdate()
      {
        foreach (Character character in Characters)
        {
          HandleLevelBounds(character);
        }
      }

      private void HandleLevelBounds(Character character)
      {
        if (character.CurrentStateType != typeof(AliveState))
        {
          return;
        }
        if (Bounds.size != Vector3.zero)
        {
          if (Top != BoundsBehaviour.Nothing && character.Controller.ColliderTopPosition.y > Bounds.max.y)
          {
            _constrainedPosition.x = character.transform.position.x;
            _constrainedPosition.y = Bounds.max.y - Mathf.Abs(character.Controller.ColliderSize.y) / 2;
            ApplyBoundsBehaviour(character, Top, _constrainedPosition);
          }

          if (Bottom != BoundsBehaviour.Nothing && character.Controller.ColliderBottomPosition.y < Bounds.min.y)
          {
            _constrainedPosition.x = character.transform.position.x;
            _constrainedPosition.y = Bounds.min.y + Mathf.Abs(character.Controller.ColliderSize.y) / 2;
            ApplyBoundsBehaviour(character, Bottom, _constrainedPosition);
          }

          if (Right != BoundsBehaviour.Nothing && character.Controller.ColliderRightPosition.x > Bounds.max.x)
          {
            _constrainedPosition.x = Bounds.max.x - Mathf.Abs(character.Controller.ColliderSize.x) / 2;
            _constrainedPosition.y = character.transform.position.y;
            ApplyBoundsBehaviour(character, Right, _constrainedPosition);
          }

          if (Left != BoundsBehaviour.Nothing && character.Controller.ColliderLeftPosition.x < Bounds.min.x)
          {
            _constrainedPosition.x = Bounds.min.x + Mathf.Abs(character.Controller.ColliderSize.x) / 2;
            _constrainedPosition.y = character.transform.position.y;
            ApplyBoundsBehaviour(character, Left, _constrainedPosition);
          }
        }
      }

      private void ApplyBoundsBehaviour(Character character, BoundsBehaviour Behaviour, Vector2 constrainedPosition)
      {
        if (Behaviour == BoundsBehaviour.Kill)
        {
          character.transform.position = constrainedPosition;
          character.Kill();
        }
        else if (Behaviour == BoundsBehaviour.Constrain)
        {
          character.transform.position = constrainedPosition;
        }
      }

#if UNITY_EDITOR
      [ButtonMethod]
      private void GeneratePolygonCollider2D()
      {
        PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();
        Vector2[] points = new Vector2[]
        {
        new Vector2(Bounds.min.x, Bounds.min.y),
        new Vector2(Bounds.min.x, Bounds.max.y),
        new Vector2(Bounds.max.x, Bounds.max.y),
        new Vector2(Bounds.max.x, Bounds.min.y),
        };
        collider.SetPath(0, points);
      }

      private void OnDrawGizmos()
      {
        var b = Bounds;
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Gizmos.DrawLine(p5, p6);
        Gizmos.DrawLine(p6, p7);
        Gizmos.DrawLine(p7, p8);
        Gizmos.DrawLine(p8, p5);

        // sides
        Gizmos.DrawLine(p1, p5);
        Gizmos.DrawLine(p2, p6);
        Gizmos.DrawLine(p3, p7);
        Gizmos.DrawLine(p4, p8);
      }
#endif

    }

  }
}