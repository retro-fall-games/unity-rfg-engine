using UnityEngine;

namespace RFG.Core
{
  [CreateAssetMenu(fileName = "New Float Reference", menuName = "RFG/Core/References/Float Reference")]
  public class FloatReference : ScriptableObject
  {
    public float Value;
  }
}