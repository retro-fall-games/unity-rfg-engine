using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RFG.Utils;

namespace RFG.Managers
{
  [CustomEditor(typeof(GameManager))]
  [CanEditMultipleObjects]
  public class GameManagerInspector : Editor
  {

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      DrawDefaultInspector();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("This will create any Tags, Layers, and Sorting Layers needed", MessageType.Warning, true);
      if (GUILayout.Button("Create Tags, Layers, and Sorting Layers"))
      {
        Setup();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private void Setup()
    {
      RFG.Utils.Setup.CheckTags(new string[] { "Player" });
      RFG.Utils.Setup.CheckLayers(new string[] { "Player", "Platforms" });
      // RFG.Utils.Setup.CheckSortLayers(new string[] { "Player" });
    }

  }

}