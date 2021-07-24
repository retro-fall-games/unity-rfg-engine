#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace RFG
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
      RFG.Setup.CheckTags(new string[] { "Player", "Checkpoint", "Warp", "Level Portal", "Trigger", "AI Character" });
      RFG.Setup.CheckLayers(new string[] { "Player", "Platforms", "OneWayPlatforms", "MovingPlatforms", "OneWayMovingPlatforms", "Stairs", "AI Character", "AI Edge Colliders" });
      RFG.Setup.CheckSortLayers(new string[] { "Background", "Foreground", "UI" });
    }

  }

}
#endif