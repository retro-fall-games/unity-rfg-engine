using System.Collections;
using UnityEngine;
using RFG;
using RFG.Platformer;

namespace Game
{
  public class Level5BossDefeatCutscene : Cutscene
  {
    [Header("AI Brains")]
    public Character player;

    [Header("Camera")]
    public Animator CameraAnimator;

    [Header("PlaceTilesTimed")]
    public PlaceTilesTimed PlaceTilesTimed;

    [Header("Level Portal")]
    public GameObject levelPortal;

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

      player.Controller.enabled = false;

      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "Boss Defeated", 1.5f);

      yield return new WaitForSeconds(2f);

      CameraAnimator.SetBool("Cutscene2", true);

      PlaceTilesTimed.IsPlacing = true;

      yield return new WaitUntil(() => PlaceTilesTimed.IsPlacing == false);

      yield return new WaitForSeconds(2f);

      levelPortal.SetActive(true);
      PlaceTilesTimed.RemoveTiles();

      yield return new WaitForSeconds(2f);

      CameraAnimator.SetBool("Cutscene2", false);

      yield return new WaitForSeconds(2f);

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