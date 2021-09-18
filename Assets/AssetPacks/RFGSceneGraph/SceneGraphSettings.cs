using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace RFG
{
  namespace SceneGraph
  {
    class SceneGraphSettings : ScriptableObject
    {
      public GUIStyle nodeStyle;
      public GUIStyle selectedNodeStyle;
      public GUIStyle inPointStyle;
      public GUIStyle outPointStyle;

      static SceneGraphSettings FindSettings()
      {
        var guids = AssetDatabase.FindAssets("t:SceneGraphSettings");
        if (guids.Length > 1)
        {
          Debug.LogWarning($"Found multiple settings files, using the first.");
        }

        switch (guids.Length)
        {
          case 0:
            return null;
          default:
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<SceneGraphSettings>(path);
        }
      }

      internal static SceneGraphSettings GetOrCreateSettings()
      {
        var settings = FindSettings();
        if (settings == null)
        {
          settings = ScriptableObject.CreateInstance<SceneGraphSettings>();

          settings.nodeStyle = new GUIStyle();
          settings.nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
          settings.nodeStyle.border = new RectOffset(12, 12, 12, 12);

          settings.selectedNodeStyle = new GUIStyle();
          settings.selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
          settings.selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

          settings.inPointStyle = new GUIStyle();
          settings.inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
          settings.inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
          settings.inPointStyle.border = new RectOffset(4, 4, 12, 12);

          settings.outPointStyle = new GUIStyle();
          settings.outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
          settings.outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
          settings.outPointStyle.border = new RectOffset(4, 4, 12, 12);

          AssetDatabase.CreateAsset(settings, "Assets/SceneGraphSettings.asset");
          AssetDatabase.SaveAssets();
        }
        return settings;
      }

      internal static SerializedObject GetSerializedSettings()
      {
        return new SerializedObject(GetOrCreateSettings());
      }
    }

    // Register a SettingsProvider using UIElements for the drawing framework:
    static class MyCustomSettingsUIElementsRegister
    {
      [SettingsProvider]
      public static SettingsProvider CreateMyCustomSettingsProvider()
      {
        // First parameter is the path in the Settings window.
        // Second parameter is the scope of this setting: it only appears in the Settings window for the Project scope.
        var provider = new SettingsProvider("Project/MyCustomUIElementsSettings", SettingsScope.Project)
        {
          label = "BehaviourTree",
          // activateHandler is called when the user clicks on the Settings item in the Settings window.
          activateHandler = (searchContext, rootElement) =>
          {
            var settings = SceneGraphSettings.GetSerializedSettings();

            // rootElement is a VisualElement. If you add any children to it, the OnGUI function
            // isn't called because the SettingsProvider uses the UIElements drawing framework.
            var title = new Label()
            {
              text = "Behaviour Tree Settings"
            };
            title.AddToClassList("title");
            rootElement.Add(title);

            var properties = new VisualElement()
            {
              style =
                {
                flexDirection = FlexDirection.Column
                }
            };
            properties.AddToClassList("property-list");
            rootElement.Add(properties);

            properties.Add(new InspectorElement(settings));

            rootElement.Bind(settings);
          },
        };

        return provider;
      }
    }
  }
}