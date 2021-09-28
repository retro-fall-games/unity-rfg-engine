using System;
using UnityEngine;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RFG.SceneGraph
{
  [AddComponentMenu("RFG/Scene/Scene Bounds")]
  public class SceneBounds : MonoBehaviour
  {
    [Header("Settings")]
    public SceneNode SceneNode;

    public event Action<Vector2, Transform> OnBoundsTop;
    public event Action<Vector2, Transform> OnBoundsBottom;
    public event Action<Vector2, Transform> OnBoundsLeft;
    public event Action<Vector2, Transform> OnBoundsRight;


    [HideInInspector]
    private Vector2 _constrainedPosition = Vector2.zero;
    private Transform _transform;

    private void Awake()
    {
      _transform = transform;
    }

    public void HandleLevelBounds(Transform _transform, Vector2 size, float minX, float maxX, float minY, float maxY)
    {
      if (SceneNode.Bounds.size == Vector3.zero)
        return;

      if (OnBoundsTop != null && maxY > SceneNode.Bounds.max.y)
      {
        _constrainedPosition.x = transform.position.x;
        _constrainedPosition.y = SceneNode.Bounds.max.y - Mathf.Abs(size.y) / 2;
        ApplyBoundsBehaviour(OnBoundsTop, _constrainedPosition, _transform);
      }

      if (OnBoundsBottom != null && minY < SceneNode.Bounds.min.y)
      {
        _constrainedPosition.x = _transform.position.x;
        _constrainedPosition.y = SceneNode.Bounds.min.y + Mathf.Abs(size.y) / 2;
        ApplyBoundsBehaviour(OnBoundsBottom, _constrainedPosition, _transform);
      }

      if (OnBoundsRight != null && maxX > SceneNode.Bounds.max.x)
      {
        _constrainedPosition.x = SceneNode.Bounds.max.x - Mathf.Abs(size.x) / 2;
        _constrainedPosition.y = _transform.position.y;
        ApplyBoundsBehaviour(OnBoundsRight, _constrainedPosition, _transform);
      }

      if (OnBoundsLeft != null && minX < SceneNode.Bounds.min.x)
      {
        _constrainedPosition.x = SceneNode.Bounds.min.x + Mathf.Abs(size.x) / 2;
        _constrainedPosition.y = _transform.position.y;
        ApplyBoundsBehaviour(OnBoundsLeft, _constrainedPosition, _transform);
      }

    }

    private void ApplyBoundsBehaviour(Action<Vector2, Transform> action, Vector2 constrainedPosition, Transform _transform)
    {
      action?.Invoke(constrainedPosition, _transform);
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void CopyFromSelection()
    {
      PolygonCollider2D collider = Selection.activeGameObject.GetComponent<PolygonCollider2D>();
      if (collider != null)
      {
        SceneNode.Bounds.min = new Vector3(collider.points[0].x, collider.points[0].y, 0);
        SceneNode.Bounds.max = new Vector3(collider.points[2].x, collider.points[2].y, 0);
      }
      EditorUtility.SetDirty(gameObject);
    }

    [ButtonMethod]
    private void GeneratePolygonCollider2DToSelection()
    {
      PolygonCollider2D collider = Selection.activeGameObject.AddComponent<PolygonCollider2D>();
      Vector2[] points = new Vector2[]
      {
        new Vector2(SceneNode.Bounds.min.x, SceneNode.Bounds.min.y),
        new Vector2(SceneNode.Bounds.min.x, SceneNode.Bounds.max.y),
        new Vector2(SceneNode.Bounds.max.x, SceneNode.Bounds.max.y),
        new Vector2(SceneNode.Bounds.max.x, SceneNode.Bounds.min.y),
      };
      collider.SetPath(0, points);
      EditorUtility.SetDirty(Selection.activeGameObject);
    }

    private void OnDrawGizmos()
    {
      var b = SceneNode.Bounds;
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