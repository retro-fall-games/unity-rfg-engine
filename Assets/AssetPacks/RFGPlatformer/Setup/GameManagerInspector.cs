using UnityEngine;
using UnityEditor;
using RFG.Core;

namespace RFG.Platformer
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
      EditorGUILayout.LabelField("Platformer Setup", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("This will create any Tags, Layers, and Sorting Layers needed for a Platformer Game", MessageType.Warning, true);
      if (GUILayout.Button("Create Tags, Layers, and Sorting Layers"))
      {
        Setup();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private void Setup()
    {
      RFG.Utils.Setup.CheckTags(new string[] { "Player" });
      RFG.Utils.Setup.CheckLayers(new string[] { "Player", "Platforms", "OneWayPlatforms", "MovingPlatforms", "OneWayMovingPlatforms", "Stairs" });
      // RFG.Utils.Setup.CheckSortLayers(new string[] { "Player" });
    }

  }

}