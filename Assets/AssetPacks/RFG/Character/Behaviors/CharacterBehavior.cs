using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG.Utils;

namespace RFG.Character
{
  public class CharacterBehavior : MonoBehaviour
  {

    protected Character _character;
    protected StateMachine<CharacterStates> _characterState;
    protected StateMachine<MovementStates> _movementState;
    protected CharacterController2D _controller;
    private void Start()
    {
      _character = GetComponent<Character>();
      _characterState = _character.characterState;
      _movementState = _character.movementState;
      _controller = _character.Controller;
    }

    public virtual void Process()
    {
    }
  }
}