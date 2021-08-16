using UnityEngine;
using UnityEditor;

namespace RFG
{
  namespace Platformer
  {
    [CustomEditor(typeof(Character))]
    [CanEditMultipleObjects]
    public class PlatformerCharacterInspector : Editor
    {

      public override void OnInspectorGUI()
      {
        serializedObject.Update();
        Character character = (Character)target;

        // if (character.CharacterState != null)
        // {
        //   EditorGUILayout.LabelField("Character State", character.CharacterState.CurrentState.ToString());
        // }
        // if (character.MovementState != null)
        // {
        //   EditorGUILayout.LabelField("Movement State", character.MovementState.CurrentState.ToString());
        // }
        // if (character.AIState != null)
        // {
        //   EditorGUILayout.LabelField("AI State", character.AIState.CurrentState.ToString());
        // }
        // if (character.AIMovementState != null)
        // {
        //   EditorGUILayout.LabelField("AI Movement State", character.AIMovementState.CurrentState.ToString());
        // }

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
        character.CharacterType = CharacterType.Player;
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

        if (character.GetComponent<Animator>() == null)
        {
          character.gameObject.AddComponent<Animator>();
        }

        CharacterController2D _controller = (character.GetComponent<CharacterController2D>() == null) ? character.gameObject.AddComponent<CharacterController2D>() : character.GetComponent<CharacterController2D>();
        _controller.platformMask = LayerMask.GetMask("Platforms");
        _controller.oneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
        _controller.movingPlatformMask = LayerMask.GetMask("MovingPlatforms");
        _controller.oneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
        _controller.stairsMask = LayerMask.GetMask("Stairs");

        if (character.GetComponent<CharacterStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterStateController>();
        }
        if (character.GetComponent<CharacterMovementStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterMovementStateController>();
        }
        if (character.GetComponent<CharacterInputController>() == null)
        {
          character.gameObject.AddComponent<CharacterInputController>();
        }
        if (character.GetComponent<CharacterAbilityController>() == null)
        {
          character.gameObject.AddComponent<CharacterAbilityController>();
        }
        if (character.GetComponent<CharacterBehaviourController>() == null)
        {
          character.gameObject.AddComponent<CharacterBehaviourController>();
        }
        if (character.GetComponent<Inventory>() == null)
        {
          character.gameObject.AddComponent<Inventory>();
        }
        if (character.GetComponent<EquipmentSet>() == null)
        {
          character.gameObject.AddComponent<EquipmentSet>();
        }
      }

      private void GenerateAICharacter()
      {
        Character character = (Character)target;
        character.CharacterType = CharacterType.AI;
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
        _controller.platformMask = LayerMask.GetMask("Platforms") | LayerMask.GetMask("AI Edge Colliders");
        _controller.oneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
        _controller.movingPlatformMask = LayerMask.GetMask("MovingPlatforms");
        _controller.oneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
        _controller.stairsMask = LayerMask.GetMask("Stairs");

        if (character.GetComponent<CharacterStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterStateController>();
        }
        if (character.GetComponent<CharacterMovementStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterMovementStateController>();
        }
        if (character.GetComponent<CharacterBehaviourController>() == null)
        {
          character.gameObject.AddComponent<CharacterBehaviourController>();
        }
        if (character.GetComponent<CharacterAIStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterAIStateController>();
        }
        if (character.GetComponent<CharacterAIMovementStateController>() == null)
        {
          character.gameObject.AddComponent<CharacterAIMovementStateController>();
        }

        // Add Behaviours
        // if (character.GetComponent<AnimationBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AnimationBehaviour>();
        // }
        // if (character.GetComponent<HealthBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<HealthBehaviour>();
        // }
        // if (character.GetComponent<AIBrainBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIBrainBehaviour>();
        // }
        // if (character.GetComponent<AIWanderingBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIWanderingBehaviour>();
        // }
        // if (character.GetComponent<AIRotateOnCollisionBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIRotateOnCollisionBehaviour>();
        // }
        // if (character.GetComponent<AIAggroBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIAggroBehaviour>();
        // }
        // if (character.GetComponent<AIJumpBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIJumpBehaviour>();
        // }
        // if (character.GetComponent<AIWeaponBehaviour>() == null)
        // {
        //   character.gameObject.AddComponent<AIWeaponBehaviour>();
        // }

        Aggro _aggro = (character.GetComponent<Aggro>() == null) ? character.gameObject.AddComponent<Aggro>() : character.GetComponent<Aggro>();

        _aggro.target1 = character.transform;
        _aggro.target2IsPlayer = true;
        _aggro.layerMask = LayerMask.GetMask("Player");


        if (character.GetComponent<Knockback>() == null)
        {
          character.gameObject.AddComponent<Knockback>();
        }

      }

    }

  }
}
