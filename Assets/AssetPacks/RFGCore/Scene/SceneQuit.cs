using UnityEngine;

namespace RFG
{
  public class SceneQuit : MonoBehaviour
  {
    public void Quit()
    {
      GameManager.Instance.Quit();
    }

  }
}
