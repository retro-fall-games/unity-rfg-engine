using UnityEngine;

namespace RFG.Utils
{
  public class CanvasSortingLayer : MonoBehaviour
  {
    public string sortingLayer;
    public Canvas canvas;

    private void Start()
    {
      canvas.sortingLayerName = sortingLayer;
    }
  }
}