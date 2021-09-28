using UnityEngine;

namespace RFG.SceneGraph
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