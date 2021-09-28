using UnityEngine;
using System.Collections.Generic;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Moveables/Parallax/Parallax Background")]
  [ExecuteInEditMode]
  public class ParallaxBackground : MonoBehaviour
  {
    public ParallaxCamera parallaxCamera;
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    private void Start()
    {
      if (parallaxCamera == null)
        parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
      if (parallaxCamera != null)
        parallaxCamera.onCameraTranslate += Move;
      SetLayers();
    }

    private void SetLayers()
    {
      parallaxLayers.Clear();
      for (int i = 0; i < transform.childCount; i++)
      {
        ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

        if (layer != null)
        {
          // layer.name = "Layer-" + i;
          parallaxLayers.Add(layer);
        }
      }
    }
    private void Move(float delta)
    {
      foreach (ParallaxLayer layer in parallaxLayers)
      {
        layer.Move(delta);
      }
    }
  }
}