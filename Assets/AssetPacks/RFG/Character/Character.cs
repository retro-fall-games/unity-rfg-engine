using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.Managers;
using RFG.Utils;

namespace RFG.Character
{
  public enum CharacterStates
  {
    Alive,
    Dead,
    Paused,
  }

  public enum MovementStates
  {
    Idle,
    Walking,
    Running,
  }

  [AddComponentMenu("RFG Engine/Character")]
  public class Character : MonoBehaviour
  {
    public StateMachine<CharacterStates> characterState;
    public StateMachine<MovementStates> movementState;
    public CharacterController2D Controller => _controller;
    public InputManager InputManager => _inputManager;
    private List<CharacterBehavior> _behaviors;
    private InputManager _inputManager;
    private CharacterController2D _controller;

    private void Awake()
    {
      characterState = new StateMachine<CharacterStates>(gameObject, true);
      movementState = new StateMachine<MovementStates>(gameObject, true);

      _behaviors = new List<CharacterBehavior>();
      _behaviors.AddRange(GetComponents<CharacterBehavior>());

      _inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;

      _controller = GetComponent<CharacterController2D>();
    }

    private void Update()
    {
      if (Time.timeScale != 0f)
      {
        ProcessBehaviors();
      }
    }

    private void ProcessBehaviors()
    {
      foreach (CharacterBehavior behavior in _behaviors)
      {
        behavior.Process();
      }
    }
  }
}