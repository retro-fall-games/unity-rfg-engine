using UnityEngine;

namespace RFG
{
  public class SceneQuit : MonoBehaviour
  {
    public void GoBackTitle()
    {
      SceneManager.Instance.LoadScene("Title", true);
    }

    public void Quit()
    {
      GameManager.Instance.Quit();
    }

  }
}
