using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RFG
{
  namespace SceneGraph
  {

    [CreateAssetMenu(fileName = "New Scene Connection", menuName = "RFG/Scene Graph/Scene Connection")]
    public class SceneConnection : ScriptableObject
    {
      [HideInInspector] public string guid;
      public SceneNode FromScene;
      public SceneNode ToScene;
    }
  }
}