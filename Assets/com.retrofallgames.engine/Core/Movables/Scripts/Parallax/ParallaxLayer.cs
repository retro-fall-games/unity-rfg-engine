using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Moveables/Parallax/Parallax Layer")]
  [ExecuteInEditMode]
  public class ParallaxLayer : MonoBehaviour
  {
    public float parallaxFactor;
    public void Move(float delta)
    {
      Vector3 newPos = transform.localPosition;
      newPos.x -= delta * parallaxFactor;
      transform.localPosition = newPos;
    }
  }
}