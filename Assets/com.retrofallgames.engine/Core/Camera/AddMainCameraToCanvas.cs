using UnityEngine;

namespace RFG.Core
{
  public class AddMainCameraToCanvas : MonoBehaviour
  {
    public Canvas canvas;
    private void Start()
    {
      canvas.worldCamera = Camera.main;
    }
  }
}