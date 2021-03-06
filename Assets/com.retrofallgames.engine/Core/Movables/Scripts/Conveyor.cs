using UnityEngine;

namespace RFG.Core
{
  [AddComponentMenu("RFG/Core/Moveables/Conveyor")]
  public class Conveyor : MonoBehaviour
  {
    [Header("Speed")]
    public float speed = 1f;

    [Header("Transform Points")]
    public Transform start;
    public Transform end;

    private void Update()
    {
      transform.Translate(-speed * Time.deltaTime, 0f, 0f);
      if (transform.position.x <= end.position.x)
      {
        transform.position = new Vector3(start.position.x, transform.position.y, transform.position.z);
      }
    }

  }
}