#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RFG
{
  namespace SceneGraph
  {
    [CustomEditor(typeof(SceneNode))]
    [CanEditMultipleObjects]
    public class SceneNodeInspector : Editor
    {
      public override void OnInspectorGUI()
      {
        serializedObject.Update();

        DrawDefaultInspector();

        SceneDropdown();
        AddConnection();

        serializedObject.ApplyModifiedProperties();
      }

      private void SceneDropdown()
      {
        SceneNode sceneNode = (SceneNode)target;
        EditorGUILayout.Space();
        List<string> options = SceneManager.GetAllScenes();
        int selected = options.FindIndex(scene => scene.Equals(sceneNode.SceneName));
        if (selected == -1)
        {
          selected = 0;
        }
        selected = EditorGUILayout.Popup("Scene", selected, options.ToArray());
        sceneNode.ChangeSceneName(options[selected]);
      }

      private void AddConnection()
      {
        SceneNode sceneNode = (SceneNode)target;
        EditorGUILayout.Space();
        if (GUILayout.Button("Add Connection"))
        {
          sceneNode.AddConnection();
        }
      }

    }
  }
}
#endif