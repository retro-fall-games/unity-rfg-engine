using UnityEngine;

namespace RFG
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