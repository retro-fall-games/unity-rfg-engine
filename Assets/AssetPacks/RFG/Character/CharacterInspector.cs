using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RFG.Utils;

namespace RFG.Character
{
  [CustomEditor(typeof(Character))]
  [CanEditMultipleObjects]
  public class CharacterInspector : Editor
  {

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      Character character = (Character)target;

      if (character.characterState != null)
      {
        EditorGUILayout.LabelField("Movement State", character.movementState.CurrentState.ToString());
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

      serializedObject.ApplyModifiedProperties();

    }

    private void GenerateCharacter()
    {
      Character character = (Character)target;

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

      BoxCollider2D _collider = (character.GetComponent<BoxCollider2D>() == null) ? character.gameObject.AddComponent<BoxCollider2D>() : character.GetComponent<BoxCollider2D>();
      _collider.isTrigger = true;

      CharacterController2D _controller = (character.GetComponent<CharacterController2D>() == null) ? character.gameObject.AddComponent<CharacterController2D>() : character.GetComponent<CharacterController2D>();
      _controller.platformMask = LayerMask.GetMask("Platforms");

      // Add behaviors
      if (character.GetComponent<CharacterWalk>() == null)
      {
        character.gameObject.AddComponent<CharacterWalk>();
      }
    }

  }

}