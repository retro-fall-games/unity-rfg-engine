using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RFG
{
  [CustomEditor(typeof(Character))]
  [CanEditMultipleObjects]
  public class CharacterInspector : Editor
  {

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      Character character = (Character)target;

      if (character.CharacterState != null)
      {
        EditorGUILayout.LabelField("Movement State", character.MovementState.CurrentState.ToString());
        // EditorGUILayout.LabelField("Condition State", character.conditionState.CurrentState.ToString());
      }

      DrawDefaultInspector();

      EditorGUILayout.Space();
      EditorGUILayout.LabelField("Autobuild", EditorStyles.boldLabel);
      EditorGUILayout.HelpBox("The Character Autobuild button will automatically add all the components needed.", MessageType.Warning, true);
      if (GUILayout.Button("Autobuild Player Character"))
      {
        GenerateCharacter();
      }

      EditorGUILayout.Space();
      EditorGUILayout.HelpBox("The AI Autobuild button will automatically add all the components needed.", MessageType.Warning, true);
      if (GUILayout.Button("Autobuild AI Character"))
      {
        GenerateAICharacter();
      }

      serializedObject.ApplyModifiedProperties();

    }

    private void GenerateCharacter()
    {
      Character character = (Character)target;
      character.characterType = CharacterType.Player;
      character.gameObject.layer = LayerMask.NameToLayer("Player");
      character.gameObject.tag = "Player";

      Rigidbody2D _rigidbody = (character.GetComponent<Rigidbody2D>() == null) ? character.gameObject.AddComponent<Rigidbody2D>() : character.GetComponent<Rigidbody2D>();
      _rigidbody.useAutoMass = false;
      _rigidbody.mass = 1;
      _rigidbody.drag = 0;
      _rigidbody.angularDrag = 0.05f;
      _rigidbody.gravityScale = 1;
      _rigidbody.interpolation = RigidbodyInterpolation2D.None;
      _rigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
      _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
      _rigidbody.isKinematic = true;
      _rigidbody.simulated = true;

      BoxCollider2D _collider = (character.GetComponent<BoxCollider2D>() == null) ? character.gameObject.AddComponent<BoxCollider2D>() : character.GetComponent<BoxCollider2D>();
      _collider.isTrigger = true;

      CharacterController2D _controller = (character.GetComponent<CharacterController2D>() == null) ? character.gameObject.AddComponent<CharacterController2D>() : character.GetComponent<CharacterController2D>();
      _controller.platformMask = LayerMask.GetMask("Platforms");
      _controller.oneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
      _controller.movingPlatformMask = LayerMask.GetMask("MovingPlatforms");
      _controller.oneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
      _controller.stairsMask = LayerMask.GetMask("Stairs");

      // Add Behaviours
      if (character.GetComponent<CharacterInput>() == null)
      {
        character.gameObject.AddComponent<CharacterInput>();
      }
      if (character.GetComponent<HorizontalMovementBehaviour>() == null)
      {
        character.gameObject.AddComponent<HorizontalMovementBehaviour>();
      }
      if (character.GetComponent<JumpBehaviour>() == null)
      {
        character.gameObject.AddComponent<JumpBehaviour>();
      }
      if (character.GetComponent<WallClingingBehaviour>() == null)
      {
        character.gameObject.AddComponent<WallClingingBehaviour>();
      }
      if (character.GetComponent<WallJumpBehaviour>() == null)
      {
        character.gameObject.AddComponent<WallJumpBehaviour>();
      }
      if (character.GetComponent<DashBehaviour>() == null)
      {
        character.gameObject.AddComponent<DashBehaviour>();
      }
      if (character.GetComponent<AnimationBehaviour>() == null)
      {
        character.gameObject.AddComponent<AnimationBehaviour>();
      }
      if (character.GetComponent<HealthBehaviour>() == null)
      {
        character.gameObject.AddComponent<HealthBehaviour>();
      }
    }

    private void GenerateAICharacter()
    {
      Character character = (Character)target;
      character.characterType = CharacterType.AI;
      character.gameObject.layer = LayerMask.NameToLayer("AI Character");
      character.gameObject.tag = "AI Character";

      Rigidbody2D _rigidbody = (character.GetComponent<Rigidbody2D>() == null) ? character.gameObject.AddComponent<Rigidbody2D>() : character.GetComponent<Rigidbody2D>();
      _rigidbody.useAutoMass = false;
      _rigidbody.mass = 1;
      _rigidbody.drag = 0;
      _rigidbody.angularDrag = 0.05f;
      _rigidbody.gravityScale = 1;
      _rigidbody.interpolation = RigidbodyInterpolation2D.None;
      _rigidbody.sleepMode = RigidbodySleepMode2D.NeverSleep;
      _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
      _rigidbody.isKinematic = true;
      _rigidbody.simulated = true;

      BoxCollider2D _collider = (character.GetComponent<BoxCollider2D>() == null) ? character.gameObject.AddComponent<BoxCollider2D>() : character.GetComponent<BoxCollider2D>();
      _collider.isTrigger = true;

      CharacterController2D _controller = (character.GetComponent<CharacterController2D>() == null) ? character.gameObject.AddComponent<CharacterController2D>() : character.GetComponent<CharacterController2D>();
      _controller.platformMask = LayerMask.GetMask("Platforms");
      _controller.oneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
      _controller.movingPlatformMask = LayerMask.GetMask("MovingPlatforms");
      _controller.oneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
      _controller.stairsMask = LayerMask.GetMask("Stairs");

      // Add Behaviours
      if (character.GetComponent<AnimationBehaviour>() == null)
      {
        character.gameObject.AddComponent<AnimationBehaviour>();
      }
      if (character.GetComponent<HealthBehaviour>() == null)
      {
        character.gameObject.AddComponent<HealthBehaviour>();
      }
      if (character.GetComponent<JumpBehaviour>() == null)
      {
        character.gameObject.AddComponent<JumpBehaviour>();
      }

      // Add Tick State Machine
      if (character.GetComponent<TickStateMachine>() == null)
      {
        character.gameObject.AddComponent<TickStateMachine>();
      }
    }

  }

}