using System.Collections;
using UnityEngine;
using RFG;

namespace Game
{
  public class IntroCutscene : Cutscene
  {
    [Header("Characters")]
    public PlatformerCharacter player;
    public PlatformerCharacter boss;

    public SpriteRenderer sword;
    public Sprite emptySword;

    protected override void Awake()
    {
      base.Awake();
      SetCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
      // Loading
      MovementPath playerMovementPath = player.FindBehaviour<AIMovementPathBehaviour>().movementPath;
      MovementPath bossMovementPath = boss.FindBehaviour<AIMovementPathBehaviour>().movementPath;
      yield return new WaitUntil(() => Dialog.Instance != null);
      Dialog.Instance.ClearAllSpeakers();

      // Player starts the movement path
      player.AIState.ChangeState(AIStates.MovementPath);
      yield return new WaitUntil(() => playerMovementPath.ReachedEnd);
      player.AIState.ChangeState(AIStates.Idle);

      // Player Talks
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "Finally, I found the sword of light", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "Now I can save the village from the darkness", 1.5f);

      // Boss starts the movement path
      boss.AIState.ChangeState(AIStates.MovementPath);
      yield return new WaitUntil(() => bossMovementPath.ReachedEnd);
      boss.AIState.ChangeState(AIStates.Idle);

      // Boss Talks
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker2, "Ha ha ha ha ha ....", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker2, "Thanks for finding this sword for me", 1.5f);
      sword.sprite = emptySword;
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker2, "You should probably get back to your village", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker2, "Now there is no more meaning", 1.5f);
      bossMovementPath.Reverse();
      bossMovementPath.Reset();
      boss.AIState.ChangeState(AIStates.MovementPath);
      yield return new WaitUntil(() => bossMovementPath.ReachedEnd);
      boss.AIState.ChangeState(AIStates.Idle);

      // Player Talks
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "I have come too far to fail", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "I will fight the evil", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "Retrieve the sword of light", 1.5f);
      yield return Dialog.Instance.Speak(Dialog.Speaker.Speaker1, "And save the village", 1.5f);

      yield return new WaitForSeconds(2f);

      // Stop everything and go play the game
      SoundManager.Instance.StopAll(true);
      yield return new WaitForSeconds(4f);
      SceneManager.Instance.LoadScene("Level1");
    }

    protected override void OnSkipEnter()
    {
      Dialog.Instance.ClearAllSpeakers();
      SoundManager.Instance.StopAll(true);
    }

    protected override void OnSkipExit()
    {
      SceneManager.Instance.LoadScene("Level1");
    }

  }
}