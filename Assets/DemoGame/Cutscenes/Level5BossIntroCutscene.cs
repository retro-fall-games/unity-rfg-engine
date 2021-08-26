using System.Collections;
using UnityEngine;
using RFG;
using RFG.Platformer;
using Cinemachine;

namespace Game
{
  public class Level5BossIntroCutscene : Cutscene
  {
    [Header("AI Brains")]
    public Character player;
    public Character boss;

    [Header("Camera")]
    public Animator CameraAnimator;

    [Header("Edge Colliders")]
    public GameObject EdgeCollider;

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

      // Disable both player and boss
      player.Controller.enabled = false;
      boss.Controller.enabled = false;

      // Player Talks
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "What's that noise?", 1.5f);

      // Play engine sound
      yield return new WaitForSeconds(2f);

      // Follow the boss with the camera
      CameraAnimator.SetBool("Cutscene1", true);

      // Play engine animations clip
      yield return new WaitForSeconds(3f);

      // Enable the boss and add force to go towards player
      EdgeCollider.SetActive(false);
      boss.Controller.enabled = true;
      boss.Controller.SetHorizontalForce(-10f);
      yield return new WaitForSeconds(2f);

      // Follow the player
      CameraAnimator.SetBool("Cutscene1", false);
      player.Controller.enabled = true;

    }

    protected override void OnSkipEnter()
    {

    }

    protected override void OnSkipExit()
    {

    }

  }
}