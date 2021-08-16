
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  namespace Platformer
  {
    [AddComponentMenu("RFG/Platformer/Character/Character Behaviour Contoller")]
    public class CharacterBehaviourController : MonoBehaviour
    {
      public class BehaviourContext
      {
        public CharacterBehaviourController controller;
        public Transform transform;
        public Character character;
      }

      public List<CharacterBehaviour> Behaviours;
      public bool CreateNewInstances = false;

      [HideInInspector]
      private BehaviourContext _behaviourContext;

      private void Awake()
      {
        _behaviourContext = new BehaviourContext();
        _behaviourContext.controller = this;
        _behaviourContext.transform = transform;
        _behaviourContext.character = GetComponent<Character>();
        if (Behaviours == null)
        {
          Behaviours = new List<CharacterBehaviour>();
        }
        if (CreateNewInstances)
        {
          List<CharacterBehaviour> tempList = Behaviours;
          Behaviours = new List<CharacterBehaviour>();
          foreach (CharacterBehaviour behaviour in tempList)
          {
            var newBehaviour = CreateInstance(behaviour.GetType());
            newBehaviour.InitValues(behaviour);
            AddBehaviour(newBehaviour);
          }
        }
      }

      private void Start()
      {
        InitBehaviours();
      }

      public void AddBehaviour(CharacterBehaviour behaviour)
      {
        behaviour.Init(_behaviourContext);
        Behaviours.Add(behaviour);
      }

      public void RemoveBehaviour(CharacterBehaviour behaviour)
      {
        behaviour.Remove(_behaviourContext);
        Behaviours.Remove(behaviour);
      }

      public T FindBehavior<T>() where T : CharacterBehaviour
      {
        foreach (CharacterBehaviour behaviour in Behaviours)
        {
          if (behaviour is T characterBehaviour)
          {
            return characterBehaviour;
          }
        }
        return null;
      }

      public void Process()
      {
        EarlyProcessBehaviours();
        if (Time.timeScale != 0f)
        {
          ProcessBehaviours();
          LateProcessBehaviours();
        }
      }

      private void InitBehaviours()
      {
        foreach (CharacterBehaviour behaviour in Behaviours)
        {
          behaviour.Init(_behaviourContext);
        }
      }

      private void EarlyProcessBehaviours()
      {
        foreach (CharacterBehaviour behaviour in Behaviours)
        {
          behaviour.EarlyProcess(_behaviourContext);
        }
      }

      private void ProcessBehaviours()
      {
        foreach (CharacterBehaviour behaviour in Behaviours)
        {
          behaviour.Process(_behaviourContext);
        }
      }

      private void LateProcessBehaviours()
      {
        foreach (CharacterBehaviour behaviour in Behaviours)
        {
          behaviour.LateProcess(_behaviourContext);
        }
      }

      public CharacterBehaviour CreateInstance(Type type)
      {
        CharacterBehaviour behaviour = (CharacterBehaviour)ScriptableObject.CreateInstance(type);
        behaviour.name = type.GetType().ToString();
        return behaviour;
      }
    }
  }
}