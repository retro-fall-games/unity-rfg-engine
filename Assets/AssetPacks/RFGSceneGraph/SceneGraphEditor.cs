using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;

namespace RFG
{
  namespace SceneGraph
  {
    public class SceneGraphEditor : EditorWindow
    {
      private SceneGraph sceneGraph;
      private Door selectedInPoint;
      private Door selectedOutPoint;

      private Vector2 offset;
      private Vector2 drag;


      [MenuItem("RFG/Scene Graph Editor")]
      private static void OpenWindow()
      {
        SceneGraphEditor window = GetWindow<SceneGraphEditor>();
        window.titleContent = new GUIContent("Scene Graph Editor");
      }

      [OnOpenAsset]
      public static bool OnOpenAsset(int instanceId, int line)
      {
        if (Selection.activeObject is SceneGraph)
        {
          OpenWindow();
          return true;
        }
        return false;
      }

      private void OnGUI()
      {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        // DrawNodes();
        // DrawConnections();

        // DrawConnectionLine(Event.current);

        // ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (sceneGraph == null)
        {
          OnSelectionChange();
        }
        else
        {
          SelectSceneGraph(sceneGraph);
        }

        if (GUI.changed) Repaint();

      }

      private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
      {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        // offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
          Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
          Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
      }



      private void ProcessEvents(Event e)
      {
        drag = Vector2.zero;

        switch (e.type)
        {
          case EventType.MouseDown:
            if (e.button == 0)
            {
              ClearConnectionSelection();
            }

            if (e.button == 1)
            {
              ProcessContextMenu(e.mousePosition);
            }
            break;

          case EventType.MouseDrag:
            if (e.button == 0)
            {
              // sceneGraph.OnDrag(e.delta);
            }
            break;
        }
      }

      // private void OnDrag(Vector2 delta)
      // {
      //   drag = delta;

      //   if (nodes != null)
      //   {
      //     for (int i = 0; i < nodes.Count; i++)
      //     {
      //       nodes[i].Drag(delta);
      //     }
      //   }

      //   GUI.changed = true;
      // }



      private void DrawConnectionLine(Event e)
      {
        if (selectedInPoint != null && selectedOutPoint == null)
        {
          Handles.DrawBezier(
              selectedInPoint.rect.center,
              e.mousePosition,
              selectedInPoint.rect.center + Vector2.left * 50f,
              e.mousePosition - Vector2.left * 50f,
              Color.white,
              null,
              2f
          );

          GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
          Handles.DrawBezier(
              selectedOutPoint.rect.center,
              e.mousePosition,
              selectedOutPoint.rect.center - Vector2.left * 50f,
              e.mousePosition + Vector2.left * 50f,
              Color.white,
              null,
              2f
          );

          GUI.changed = true;
        }
      }

      private void ProcessContextMenu(Vector2 mousePosition)
      {
        GenericMenu genericMenu = new GenericMenu();
        // genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
      }



      // private void OnClickAddNode(Vector2 mousePosition)
      // {
      //   if (nodes == null)
      //   {
      //     nodes = new List<SceneNode>();
      //   }

      //   nodes.Add(new SceneNode(mousePosition, 200, 50, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
      // }

      private void OnClickInPoint(Door inPoint)
      {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
          if (selectedOutPoint.node != selectedInPoint.node)
          {
            // CreateConnection();
            ClearConnectionSelection();
          }
          else
          {
            ClearConnectionSelection();
          }
        }
      }

      private void OnClickOutPoint(Door outPoint)
      {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
          if (selectedOutPoint.node != selectedInPoint.node)
          {
            // CreateConnection();
            ClearConnectionSelection();
          }
          else
          {
            ClearConnectionSelection();
          }
        }
      }

      // private void OnClickRemoveNode(SceneNode node)
      // {
      //   if (connections != null)
      //   {
      //     List<DoorConnection> connectionsToRemove = new List<DoorConnection>();

      //     for (int i = 0; i < connections.Count; i++)
      //     {
      //       if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
      //       {
      //         connectionsToRemove.Add(connections[i]);
      //       }
      //     }

      //     for (int i = 0; i < connectionsToRemove.Count; i++)
      //     {
      //       connections.Remove(connectionsToRemove[i]);
      //     }

      //     connectionsToRemove = null;
      //   }

      //   nodes.Remove(node);
      // }

      // private void OnClickRemoveConnection(DoorConnection connection)
      // {
      //   connections.Remove(connection);
      // }

      // private void CreateConnection()
      // {
      //   if (connections == null)
      //   {
      //     connections = new List<DoorConnection>();
      //   }

      //   connections.Add(new DoorConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
      // }

      private void ClearConnectionSelection()
      {
        selectedInPoint = null;
        selectedOutPoint = null;
      }

      private void OnPlayModeStateChanged(PlayModeStateChange obj)
      {
        switch (obj)
        {
          case PlayModeStateChange.EnteredEditMode:
            OnSelectionChange();
            break;
          case PlayModeStateChange.ExitingEditMode:
            break;
          case PlayModeStateChange.EnteredPlayMode:
            OnSelectionChange();
            break;
          case PlayModeStateChange.ExitingPlayMode:
            break;
        }
      }

      private void OnSelectionChange()
      {
        EditorApplication.delayCall += () =>
        {
          SceneGraph sceneGraph = Selection.activeObject as SceneGraph;
          SelectSceneGraph(sceneGraph);
        };
      }

      private void SelectSceneGraph(SceneGraph sceneGraph)
      {

      }

    }
  }
}