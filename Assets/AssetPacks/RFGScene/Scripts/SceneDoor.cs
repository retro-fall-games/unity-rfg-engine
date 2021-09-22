using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace Scene
  {

    [CreateAssetMenu(fileName = "New Scene Door", menuName = "RFG/Scene Graph/Scene Door")]
    public class SceneDoor : ScriptableObject
    {
      [HideInInspector] public string guid;
      [HideInInspector] public Vector2 position;

      public SceneNode ToSceneNode;
      public SceneDoor ToSceneDoor;

    }
  }
}