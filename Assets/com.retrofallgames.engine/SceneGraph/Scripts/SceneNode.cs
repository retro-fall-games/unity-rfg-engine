using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RFG.SceneGraph
{

  [CreateAssetMenu(fileName = "New Scene Node", menuName = "RFG/Scene Graph/Scene Node")]
  public class SceneNode : ScriptableObject
  {
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public string SceneName;
    [HideInInspector] public List<SceneDoor> SceneDoors = new List<SceneDoor>();

    public Bounds Bounds = new Bounds(Vector3.zero, Vector3.one * 10);



#if UNITY_EDITOR

    public void ChangeSceneName(string sceneName)
    {
      if (!sceneName.Equals(SceneName))
      {
        SceneName = sceneName;
        EditorUtility.SetDirty(this);
      }
    }

    public SceneDoor CreateSceneDoor()
    {
      SceneDoor door = ScriptableObject.CreateInstance<SceneDoor>();
      door.name = "SceneDoor";
      door.guid = GUID.Generate().ToString();
      SceneDoors.Add(door);

      if (!Application.isPlaying)
      {
        AssetDatabase.AddObjectToAsset(door, this);
      }
      AssetDatabase.SaveAssets();

      EditorUtility.SetDirty(this);
      return door;
    }

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