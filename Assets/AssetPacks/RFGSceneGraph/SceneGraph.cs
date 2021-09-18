using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace SceneGraph
  {

    [CreateAssetMenu(fileName = "New Scene Graph", menuName = "RFG/Scene Graph/Scene Graph")]
    public class SceneGraph : ScriptableObject
    {
      public List<SceneNode> nodes;
      public List<DoorConnection> connections;



#if UNITY_EDITOR

      private void DrawNodes()
      {
        if (nodes != null)
        {
          for (int i = 0; i < nodes.Count; i++)
          {
            nodes[i].Draw();
          }
        }
      }

      private void DrawConnections()
      {
        if (connections != null)
        {
          for (int i = 0; i < connections.Count; i++)
          {
            connections[i].Draw();
          }
        }
      }

      private void ProcessNodeEvents(Event e)
      {
        if (nodes != null)
        {
          for (int i = nodes.Count - 1; i >= 0; i--)
          {
            bool guiChanged = nodes[i].ProcessEvents(e);

            if (guiChanged)
            {
              GUI.changed = true;
            }
          }
        }
      }



      public SceneNode CreateSceneNode()
      {
        if (nodes == null)
        {
          nodes = new List<SceneNode>();
        }

        SceneNode node = ScriptableObject.CreateInstance<SceneNode>();
        node.guid = GUID.Generate().ToString();

        // node.rect = new Rect(position.x, position.y, width, height);

        SceneGraphSettings settings = SceneGraphSettings.GetOrCreateSettings();

        // nodeStyle = settings.nodeStyle;
        // inPoint = new Door(this, ConnectionPointType.In, OnClickInPoint);
        // outPoint = new Door(this, ConnectionPointType.Out, OnClickOutPoint);
        // defaultNodeStyle = settings.nodeStyle;
        // selectedNodeStyle = settings.selectedNodeStyle;
        // OnRemoveNode = OnClickRemoveNode;


        Undo.RecordObject(this, "Scene Graph (CreateSceneNode)");
        nodes.Add(node);

        if (!Application.isPlaying)
        {
          AssetDatabase.AddObjectToAsset(node, this);
        }
        Undo.RegisterCreatedObjectUndo(node, "Scene Graph (CreateSceneNode)");
        AssetDatabase.SaveAssets();

        return node;
      }

      public void DeleteSceneNode(SceneNode node)
      {
        Undo.RecordObject(this, "Scene Graph (DeleteSceneNode)");
        nodes.Remove(node);
        // AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
      }

#endif

    }
  }
}