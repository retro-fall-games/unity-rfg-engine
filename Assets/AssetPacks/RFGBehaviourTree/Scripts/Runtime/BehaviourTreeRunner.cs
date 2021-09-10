using UnityEngine;

namespace RFG
{
  public class BehaviourTreeRunner : MonoBehaviour
  {
    public BehaviourTree tree;

    [HideInInspector]
    private INodeContext _context;

    private void Awake()
    {
      _context = GetComponent(typeof(INodeContext)) as INodeContext;
    }

    private void Start()
    {
      tree = tree.Clone();
      tree.Bind(_context);
    }

    private void Update()
    {
      if (tree)
      {
        tree.Update();
      }
    }

    private void OnDrawGizmosSelected()
    {
      if (!tree)
      {
        return;
      }

      BehaviourTree.Traverse(tree.rootNode, (n) =>
      {
        if (n.drawGizmos)
        {
          n.OnDrawGizmos();
        }
      });
    }
  }
}