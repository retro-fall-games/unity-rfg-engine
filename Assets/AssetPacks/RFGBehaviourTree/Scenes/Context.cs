using UnityEngine;

namespace ContextTest
{
  public class ContextData
  {
    public Transform transform;
  }

  public class Context : MonoBehaviour, INodeContext
  {
    public ContextData contextData;
    private void Awake()
    {
      contextData = new ContextData();
      contextData.transform = transform;
    }
  }
}
