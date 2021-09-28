using UnityEngine;
using UnityEditor;

namespace RFG.Platformer
{
  using Interactions;

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
      //   EditorGUILayout.LabelField("AI State", character.CurrentState.ToString());
      // }
      // if (character.AIMovementState != null)
      // {
      //   EditorGUILayout.LabelField("AI Movement State", character.CurrentState.ToString());
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
      _controller.PlatformMask = LayerMask.GetMask("Platforms");
      _controller.OneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
      _controller.MovingPlatformMask = LayerMask.GetMask("MovingPlatforms");
      _controller.OneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
      _controller.StairsMask = LayerMask.GetMask("Stairs");

      if (character.GetComponent<HealthBehaviour>() == null)
      {
        character.gameObject.AddComponent<HealthBehaviour>();
      }
      if (character.GetComponent<MovementAbility>() == null)
      {
        character.gameObject.AddComponent<MovementAbility>();
      }
      if (character.GetComponent<JumpAbility>() == null)
      {
        character.gameObject.AddComponent<JumpAbility>();
      }
      if (character.GetComponent<AttackAbility>() == null)
      {
        character.gameObject.AddComponent<AttackAbility>();
      }
      if (character.GetComponent<DashAbility>() == null)
      {
        character.gameObject.AddComponent<DashAbility>();
      }
      if (character.GetComponent<PauseAbility>() == null)
      {
        character.gameObject.AddComponent<PauseAbility>();
      }
      if (character.GetComponent<WallClingingAbility>() == null)
      {
        character.gameObject.AddComponent<WallClingingAbility>();
      }
      if (character.GetComponent<WallJumpAbility>() == null)
      {
        character.gameObject.AddComponent<WallJumpAbility>();
      }
      if (character.GetComponent<StairsAbility>() == null)
      {
        character.gameObject.AddComponent<StairsAbility>();
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
      _controller.PlatformMask = LayerMask.GetMask("Platforms") | LayerMask.GetMask("AI Edge Colliders");
      _controller.OneWayPlatformMask = LayerMask.GetMask("OneWayPlatforms");
      _controller.MovingPlatformMask = LayerMask.GetMask("MovingPlatforms");
      _controller.OneWayMovingPlatformMask = LayerMask.GetMask("OneWayMovingPlatforms");
      _controller.StairsMask = LayerMask.GetMask("Stairs");

      Aggro _aggro = (character.GetComponent<Aggro>() == null) ? character.gameObject.AddComponent<Aggro>() : character.GetComponent<Aggro>();

      _aggro.target1 = character.transform;
      _aggro.target2IsPlayer = true;
      _aggro.layerMask = LayerMask.GetMask("Player");
      _aggro.tags = new string[] { "Player" };

      if (character.GetComponent<Knockback>() == null)
      {
        character.gameObject.AddComponent<Knockback>();
      }
      if (character.GetComponent<EquipmentSet>() == null)
      {
        character.gameObject.AddComponent<EquipmentSet>();
      }
      if (character.GetComponent<HealthBehaviour>() == null)
      {
        character.gameObject.AddComponent<HealthBehaviour>();
      }
      if (character.GetComponent<AIBrainBehaviour>() == null)
      {
        character.gameObject.AddComponent<AIBrainBehaviour>();
      }

    }

  }

}
