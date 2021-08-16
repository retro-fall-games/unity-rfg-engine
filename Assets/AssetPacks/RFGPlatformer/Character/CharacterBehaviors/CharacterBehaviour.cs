using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    public class CharacterBehaviour : ScriptableObject
    {
      public virtual void Init(CharacterBehaviourController.BehaviourContext ctx)
      {
      }
      public virtual void InitValues(CharacterBehaviour behaviour)
      {
      }
      public virtual void Remove(CharacterBehaviourController.BehaviourContext ctx)
      {
      }
      public virtual void EarlyProcess(CharacterBehaviourController.BehaviourContext ctx)
      {
      }
      public virtual void Process(CharacterBehaviourController.BehaviourContext ctx)
      {
      }
      public virtual void LateProcess(CharacterBehaviourController.BehaviourContext ctx)
      {
      }
    }
  }
}