using UnityEngine.UIElements;

namespace RFG
{
  namespace BehaviourTree
  {
    public class SplitView : TwoPaneSplitView
    {
      public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
  }
}