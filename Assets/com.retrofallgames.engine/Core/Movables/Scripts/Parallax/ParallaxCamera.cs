using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Moveables/Parallax/Parallax Camera")]
  [ExecuteInEditMode]
  public class ParallaxCamera : MonoBehaviour
  {
    public delegate void ParallaxCameraDelegate(float deltaMovement);
    public ParallaxCameraDelegate onCameraTranslate;
    private float oldPosition;

    private void Start()
    {
      oldPosition = transform.position.x;
    }

    private void LateUpdate()
    {
      if (transform.position.x != oldPosition)
      {
        if (onCameraTranslate != null)
        {
          float delta = oldPosition - transform.position.x;
          onCameraTranslate(delta);
        }
        oldPosition = transform.position.x;
      }
    }
  }
}