using System;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace SceneGraph
  {

    [CreateAssetMenu(fileName = "New Scene Node", menuName = "RFG/Scene Graph/Scene Node")]
    public class SceneNode : ScriptableObject
    {
      [HideInInspector] public string guid;
      public Rect rect;
      public string title;
      public bool isDragged;
      public bool isSelected;

      public Door inPoint;
      public Door outPoint;

      public Action<SceneNode> OnRemoveNode;

      public GUIStyle nodeStyle;
      public GUIStyle defaultNodeStyle;
      public GUIStyle selectedNodeStyle;

      public SceneNode(Vector2 position, float width, float height, Action<Door> OnClickInPoint, Action<Door> OnClickOutPoint, Action<SceneNode> OnClickRemoveNode)
      {

      }

      public void Drag(Vector2 delta)
      {
        rect.position += delta;
      }

      public void Draw()
      {
        inPoint.Draw();
        outPoint.Draw();
        GUI.Box(rect, title, nodeStyle);
        string name = GUI.TextField(new Rect(rect.x, rect.y, 20, 20), "Hello");
      }

      public bool ProcessEvents(Event e)
      {
        switch (e.type)
        {
          case EventType.MouseDown:
            if (e.button == 0)
            {
              if (rect.Contains(e.mousePosition))
              {
                isDragged = true;
                GUI.changed = true;
                isSelected = true;
                nodeStyle = selectedNodeStyle;
              }
              else
              {
                GUI.changed = true;
                isSelected = false;
                nodeStyle = defaultNodeStyle;
              }
            }

            if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
            {
              ProcessContextMenu();
              e.Use();
            }
            break;

          case EventType.MouseUp:
            isDragged = false;
            break;

          case EventType.MouseDrag:
            if (e.button == 0 && isDragged)
            {
              Drag(e.delta);
              e.Use();
              return true;
            }
            break;
        }

        return false;
      }

      private void ProcessContextMenu()
      {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
      }

      private void OnClickRemoveNode()
      {
        if (OnRemoveNode != null)
        {
          OnRemoveNode(this);
        }
      }
    }
  }
}