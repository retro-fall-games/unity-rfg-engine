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
      public List<SceneNode> sceneNodes = new List<SceneNode>();

#if UNITY_EDITOR
      public SceneNode CreateNode(System.Type type)
      {
        SceneNode node = ScriptableObject.CreateInstance(type) as SceneNode;
        node.name = node.SceneName;
        node.guid = GUID.Generate().ToString();
        sceneNodes.Add(node);

        if (!Application.isPlaying)
        {
          AssetDatabase.AddObjectToAsset(node, this);
        }
        AssetDatabase.SaveAssets();

        return node;
      }

      public void DeleteNode(SceneNode node)
      {
        sceneNodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
      }
#endif
    }
  }
}