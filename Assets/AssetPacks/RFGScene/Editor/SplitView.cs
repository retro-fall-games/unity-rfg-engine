using UnityEngine.UIElements;

namespace RFG
{
  namespace Scene
  {
    public class SplitView : TwoPaneSplitView
    {
      public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
  }
}