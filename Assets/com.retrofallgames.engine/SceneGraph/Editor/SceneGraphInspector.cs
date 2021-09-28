using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RFG.SceneGraph
{
  [CustomEditor(typeof(SceneGraph))]
  [CanEditMultipleObjects]
  public class SceneGraphInspector : Editor
  {
    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      DrawDefaultInspector();
      // AddSceneNode();

      serializedObject.ApplyModifiedProperties();
    }

    private void AddSceneNode()
    {
      SceneGraph sceneGraph = (SceneGraph)target;
      EditorGUILayout.Space();
      if (GUILayout.Button("Add Scene Node"))
      {
        sceneGraph.CreateNode(typeof(SceneNode));
      }
    }

  }
}