using System.Collections;
using UnityEngine;
using RFG.Core;
using RFG.Platformer;
using RFG.BehaviourTree;
using RFG.SceneGraph;

namespace Game
{
  public class Level5BossIntroCutscene : Cutscene
  {
    [Header("AI Brains")]
    public Character player;
    public Character boss;
    public BehaviourTreeRunner bossBehaviourTree;

    [Header("Camera")]
    public Animator playerAnimator;
    public Animator CameraAnimator;

    [Header("PlaceTilesTimed")]
    public PlaceTilesTimed PlaceTilesTimed;

    protected override void Awake()
    {
      base.Awake();
      SetCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
      // Loading
      yield return new WaitUntil(() => Dialog.Instance != null);
      Dialog.Instance.ClearAllSpeakers();

      // Disable player
      player.Controller.SetForce(Vector2.zero);
      player.Controller.enabled = false;
      player.MovementState.ChangeState(typeof(IdleState));
      player.DisableAllAbilities();

      // Player Talks
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "What's that noise?", 1.5f);
      boss.MovementState.ChangeState(typeof(RunningState));

      PlaceTilesTimed.IsPlacing = true;

      yield return new WaitUntil(() => PlaceTilesTimed.IsPlacing == false);

      // Play engine sound
      yield return new WaitForSeconds(2f);

      CameraAnimator.SetBool("Cutscene1", true);

      // Play engine animations clip
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker2, "Level 5 Boss", 1.5f);
      yield return new WaitForSeconds(3f);

      boss.Controller.enabled = true;
      boss.Controller.SetHorizontalForce(-20f);
      bossBehaviourTree.enabled = true;
      yield return new WaitForSeconds(3f);

      // Follow the player
      CameraAnimator.SetBool("Cutscene1", false);
      player.Controller.enabled = true;
      player.EnableAllAbilities();

    }

    protected override void OnSkipEnter()
    {

    }

    protected override void OnSkipExit()
    {

    }

  }
}