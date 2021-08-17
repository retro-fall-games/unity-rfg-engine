using System;
using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Interactions/Aggro")]
  public class Aggro : MonoBehaviour
  {
    [Header("Transform Targets")]
    public Transform target1;
    public Transform target2;
    public bool target2IsPlayer;

    [Header("Controls")]
    public float minDistance = 5f;
    public bool HasAggro { get; private set; }
    public event Action<bool> OnAggroChange;

    [Header("Line Of Sight")]
    public LayerMask layerMask;

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      if (target2IsPlayer)
      {
        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
          target2 = player.transform;
        }
      }
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
      return hit;
    }

  }
}