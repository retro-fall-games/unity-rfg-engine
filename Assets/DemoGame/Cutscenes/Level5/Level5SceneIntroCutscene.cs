using System.Collections;
using UnityEngine;
using RFG;
using RFG.Platformer;

namespace Game
{
  public class Level5SceneIntroCutscene : Cutscene
  {
    [Header("AI Brains")]
    public Character boss;
    public AIBrainBehaviour bossAIBrain;

    protected override void Awake()
    {
      base.Awake();
      SetCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
      // Turn off the boss until the trigger
      // boss.Controller.enabled = false;
      // bossAIBrain.enabled = false;
      yield return null;
    }

    protected override void OnSkipEnter()
    {

    }

    protected override void OnSkipExit()
    {

    }

  }
}