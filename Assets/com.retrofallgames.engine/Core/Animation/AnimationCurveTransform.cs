using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Animation/Animation Curve Transform")]
  public class AnimationCurveTransform : MonoBehaviour
  {

    public AnimationCurve curve;
    public bool useDefaultCurve;
    public float speed = 1f;
    private float _originalY;
    private float _curveDeltaTime = 0.0f;

    private void Awake()
    {
      if (useDefaultCurve)
      {
        curve = new AnimationCurve();
        curve.AddKey(new Keyframe(0, 0, 0, 0));
        curve.AddKey(new Keyframe(0.5f, -.5f, 0, 0));
        curve.AddKey(new Keyframe(1, 0, 0, 0));
      }
    }

    private void Start()
    {
      _originalY = transform.position.y;
    }
    private void Update()
    {
      _curveDeltaTime += Time.deltaTime;
      transform.position = new Vector3(transform.position.x, _originalY + curve.Evaluate(_curveDeltaTime), transform.position.z);
      if (_curveDeltaTime > speed)
      {
        _curveDeltaTime = 0;
      }
    }
  }
}