using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG Platformer/AI/Aggro")]
  public class Aggro : MonoBehaviour
  {
    [Header("Transform Targets")]
    public Transform target1;
    public Transform target2;
    public bool target2IsPlayer = false;

    [Header("Controls")]
    public float minDistance = 5f;
    public bool HasAggro { get; private set; }
    public event System.Action<bool> OnAggroChange;

    [Header("Line Of Sight")]
    public LayerMask layerMask;
    public string[] tags;

    private void Start()
    {
      if (target2IsPlayer)
      {
        StartCoroutine(WaitForPlayer());
      }
    }

    private IEnumerator WaitForPlayer()
    {
      yield return new WaitUntil(() => LevelManager.Instance != null);
      yield return new WaitUntil(() => LevelManager.Instance.PlayerCharacter != null);
      target2 = LevelManager.Instance.PlayerCharacter.transform;
    }

    private void Update()
    {
      if (target1 != null && target2 != null)
      {
        float distance = Vector2.Distance(target1.position, target2.position);
        bool inRange = distance < minDistance;
        bool hasLineOfSight = false;
        if (!HasAggro && inRange)
        {
          hasLineOfSight = LineOfSight();
          if (hasLineOfSight)
          {
            HasAggro = true;
            OnAggroChange?.Invoke(HasAggro);
          }
        }
        else if (HasAggro)
        {
          hasLineOfSight = LineOfSight();
          if (!hasLineOfSight || !inRange)
          {
            HasAggro = false;
            OnAggroChange?.Invoke(HasAggro);
          }
        }
      }
    }

    private bool LineOfSight()
    {
      Vector2 start = target1.position;
      Vector2 direction = (target2.position - target1.position).normalized;
      float distance = Vector2.Distance(target2.position, target1.position);

      RaycastHit2D hit = RFG.Physics2D.Raycast(start, direction, distance, layerMask, Color.red);
      if (hit)
      {
        // Debug.DrawRay(start, direction * distance, Color.red);
        return CheckTags(hit.transform.gameObject);
      }
      return false;
    }

    private bool CheckTags(GameObject obj)
    {
      for (int i = 0; i < tags.Length; i++)
      {
        if (obj.CompareTag(tags[i]))
        {
          return true;
        }
      }
      return false;
    }

  }
}