using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace SceneGraph
  {

    [CreateAssetMenu(fileName = "New Scene Node", menuName = "RFG/Scene Graph/Scene Node")]
    public class SceneNode : ScriptableObject
    {
      public List<SceneConnection> sceneConnections = new List<SceneConnection>();
      [HideInInspector] public string guid;
      [HideInInspector] public Vector2 position;
      [HideInInspector] public string SceneName;

#if UNITY_EDITOR

      public void ChangeSceneName(string sceneName)
      {
        if (!sceneName.Equals(SceneName))
        {
          SceneName = sceneName;
          this.name = sceneName;
          AssetDatabase.SaveAssets();
          EditorUtility.SetDirty(this);
        }
      }

      public void AddConnection()
      {
        SceneConnection connection = ScriptableObject.CreateInstance<SceneConnection>();
        connection.name = "SceneConnection";
        connection.guid = GUID.Generate().ToString();
        sceneConnections.Add(connection);

        if (!Application.isPlaying)
        {
          AssetDatabase.AddObjectToAsset(connection, this);
        }
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(this);
      }

      // public void AddConnection(SceneNode FromScene, SceneNode ToScene)
      // {
      //   SceneConnection connection = new SceneConnection();
      //   connection.FromScene = FromScene;
      //   connection.ToScene = ToScene;
      //   sceneConnections.Add(connection);
      //   EditorUtility.SetDirty(this);
      // }

      // public void RemoveConnection(SceneNode FromScene, SceneNode ToScene)
      // {
      //   SceneConnection connection = sceneConnections.Find(c => c.FromScene == FromScene && c.ToScene == ToScene);
      //   if (connection != null)
      //   {
      //     sceneConnections.Remove(connection);
      //     EditorUtility.SetDirty(this);
      //   }
      // }
#endif

    }
  }
}