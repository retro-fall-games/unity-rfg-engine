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
      EditorGUILayout.LabelField("Sound Setup", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("This will create Tags to use with the Sound Manager", MessageType.Warning, true);
      if (GUILayout.Button("Generate Sound Setup"))
      {
        SoundSetup();
      }

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Platformer Setup", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("This will create Tags, Layers, and Sorting Layers needed for a Platformer Game", MessageType.Warning, true);
      if (GUILayout.Button("Generate Platformer Setup"))
      {
        PlatformerSetup();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private void SoundSetup()
    {
      RFG.Setup.CheckTags(new string[] { "SoundTrack", "SoundAmbience", "SoundFx" });
    }

    private void PlatformerSetup()
    {
      RFG.Setup.CheckTags(new string[] { "Player", "Checkpoint", "Warp", "Level Portal", "Trigger", "AI Character", "PickUp" });
      RFG.Setup.CheckLayers(new string[] { "Player", "Platforms", "OneWayPlatforms", "MovingPlatforms", "OneWayMovingPlatforms", "Stairs", "AI Character", "AI Edge Colliders" });
      RFG.Setup.CheckSortLayers(new string[] { "Background", "Foreground", "UI" });
    }

  }

}
#endif