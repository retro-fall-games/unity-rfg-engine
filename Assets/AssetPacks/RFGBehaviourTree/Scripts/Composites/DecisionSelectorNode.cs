using UnityEngine;

namespace RFG
{
  namespace BehaviourTree
  {
    public class DecisionSelectorNode : CompositeNode
    {
      public float DecisionTime = 3f;

      [Range(0, 1)]
      public float DecisionWeight = 0.5f;
      protected int current = 0;
      private float _decisionTimeElapsed = 0f;

      protected override void OnStart()
      {
        _decisionTimeElapsed = Time.time;
      }

      protected override void OnStop()
      {
        children.ForEach(node => node.Abort());
      }

      protected override State OnUpdate()
      {
        if (Time.time - _decisionTimeElapsed > DecisionTime)
        {
          _decisionTimeElapsed = Time.time;
          if (Random.value > DecisionWeight)
          {
            children[current].Abort();
            current = Random.Range(0, children.Count);
          }
        }

        var child = children[current];
        child.Update();
        return State.Running;
      }
    }
  }
}