using UnityEngine;

namespace RFG.Interactions
{
  interface IBreakable
  {
    void Break(RaycastHit2D hit);
  }
}