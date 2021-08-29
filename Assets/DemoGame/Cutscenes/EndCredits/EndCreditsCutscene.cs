using System.Collections;
using UnityEngine;
using RFG;

namespace Game
{
  public class EndCreditsCutscene : Cutscene
  {

    public Animator EndCreditsAnimator;

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

      yield return new WaitForSeconds(2f);

      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "You have brought peace to the village.", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "Thank you for playing Bit Sword.", 3f);

      yield return new WaitForSeconds(3f);
      yield return new WaitUntil(() => !EndCreditsAnimator.GetCurrentAnimatorStateInfo(0).IsName("EndCredits"));
      yield return new WaitForSeconds(5f);

      SceneManager.Instance.LoadScene("Title");
    }

    protected override void OnSkipEnter()
    {
    }

    protected override void OnSkipExit()
    {
      // SceneManager.Instance.LoadScene("Title");
    }

  }
}