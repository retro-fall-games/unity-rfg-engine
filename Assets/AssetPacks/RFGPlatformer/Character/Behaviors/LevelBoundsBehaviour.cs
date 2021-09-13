using UnityEngine;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Behaviours/Level Bounds")]
    public class LevelBoundsBehaviour : MonoBehaviour
    {
      public enum BoundsBehaviour { Nothing, Constrain, Kill }

      [Header("Bounds Behaviour")]
      public BoundsBehaviour Top = BoundsBehaviour.Constrain;
      public BoundsBehaviour Bottom = BoundsBehaviour.Kill;
      public BoundsBehaviour Left = BoundsBehaviour.Constrain;
      public BoundsBehaviour Right = BoundsBehaviour.Constrain;
      public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one * 10);

      [HideInInspector]
      private Vector2 _constrainedPosition = Vector2.zero;
      private Character _character;
      private CharacterController2D _controller;
      private Transform _transform;

      private void Awake()
      {
        _transform = transform;
        _character = GetComponent<Character>();
        _controller = GetComponent<CharacterController2D>();
      }

      private void LateUpdate()
      {
        if (_character.CharacterState.CurrentStateType != typeof(AliveState))
          return;
        HandleLevelBounds();
      }

      private void HandleLevelBounds()
      {
        _controller.State.TouchingLevelBounds = false;
        if (Bounds.size != Vector3.zero)
        {
          if (Top != BoundsBehaviour.Nothing && _controller.ColliderTopPosition.y > Bounds.max.y)
          {
            _controller.State.TouchingLevelBounds = true;
            _constrainedPosition.x = _transform.position.x;
            _constrainedPosition.y = Bounds.max.y - Mathf.Abs(_controller.ColliderSize.y) / 2;
            ApplyBoundsBehaviour(Top, _constrainedPosition);
          }

          if (Bottom != BoundsBehaviour.Nothing && _controller.ColliderBottomPosition.y < Bounds.min.y)
          {
            _controller.State.TouchingLevelBounds = true;
            _constrainedPosition.x = _transform.position.x;
            _constrainedPosition.y = Bounds.min.y + Mathf.Abs(_controller.ColliderSize.y) / 2;
            ApplyBoundsBehaviour(Bottom, _constrainedPosition);
          }

          if (Right != BoundsBehaviour.Nothing && _controller.ColliderRightPosition.x > Bounds.max.x)
          {
            _controller.State.TouchingLevelBounds = true;
            _constrainedPosition.x = Bounds.max.x - Mathf.Abs(_controller.ColliderSize.x) / 2;
            _constrainedPosition.y = _transform.position.y;
            ApplyBoundsBehaviour(Right, _constrainedPosition);
          }

          if (Left != BoundsBehaviour.Nothing && _controller.ColliderLeftPosition.x < Bounds.min.x)
          {
            _controller.State.TouchingLevelBounds = true;
            _constrainedPosition.x = Bounds.min.x + Mathf.Abs(_controller.ColliderSize.x) / 2;
            _constrainedPosition.y = _transform.position.y;
            ApplyBoundsBehaviour(Left, _constrainedPosition);
          }
        }
      }

      private void ApplyBoundsBehaviour(BoundsBehaviour Behaviour, Vector2 constrainedPosition)
      {
        if (Behaviour == BoundsBehaviour.Kill)
        {
          _transform.position = constrainedPosition;
          _character.Kill();
        }
        else if (Behaviour == BoundsBehaviour.Constrain)
        {
          _transform.position = constrainedPosition;
        }
      }

#if UNITY_EDITOR
      [ButtonMethod]
      private void CopyFromSelection()
      {
        LevelBoundsBehaviour levelBounds = Selection.activeGameObject.GetComponent<LevelBoundsBehaviour>();
        if (levelBounds != null)
        {
          Bounds = levelBounds.Bounds;
          Top = levelBounds.Top;
          Bottom = levelBounds.Bottom;
          Left = levelBounds.Left;
          Right = levelBounds.Right;
        }

        PolygonCollider2D collider = Selection.activeGameObject.GetComponent<PolygonCollider2D>();
        if (collider != null)
        {
          Bounds.min = new Vector3(collider.points[0].x, collider.points[0].y, 0);
          Bounds.max = new Vector3(collider.points[2].x, collider.points[2].y, 0);
        }
        EditorUtility.SetDirty(gameObject);
      }

      [ButtonMethod]
      private void GeneratePolygonCollider2DToSelection()
      {
        PolygonCollider2D collider = Selection.activeGameObject.AddComponent<PolygonCollider2D>();
        Vector2[] points = new Vector2[]
        {
        new Vector2(Bounds.min.x, Bounds.min.y),
        new Vector2(Bounds.min.x, Bounds.max.y),
        new Vector2(Bounds.max.x, Bounds.max.y),
        new Vector2(Bounds.max.x, Bounds.min.y),
        };
        collider.SetPath(0, points);
        EditorUtility.SetDirty(Selection.activeGameObject);
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