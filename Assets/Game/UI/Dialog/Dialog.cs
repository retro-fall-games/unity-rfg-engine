using UnityEngine;
using System.Collections;
using TMPro;
using RFG;
using RFGFx;

namespace Game
{
  public class Dialog : Singleton<Dialog>
  {
    public enum Speaker { Speaker1, Speaker2, Speaker3 };
    public TMP_Text speaker1;
    public TMP_Text speaker2;
    public TMP_Text speaker3;
    public float speed = 0.05f;
    public float fadeSpeed = 1f;
    private Coroutine _coroutine;

    public void Cancel()
    {
      StopCoroutine(_coroutine);
    }

    public void ClearSpeaker(Speaker speaker)
    {
      TMP_Text textRef = GetSpeaker(speaker);
      textRef.SetText("");
    }

    public void ClearAllSpeakers()
    {
      ClearSpeaker(Speaker.Speaker1);
      ClearSpeaker(Speaker.Speaker2);
      ClearSpeaker(Speaker.Speaker3);
    }

    public IEnumerator Speak(Speaker speaker, string dialog, float waitAfter = 0f, bool instant = false)
    {
      TMP_Text text = GetSpeaker(speaker);
      ClearSpeaker(speaker);
      if (instant)
      {
        text.SetText(dialog);
      }
      else
      {
        string fullDialog = "";
        yield return TextFade.FadeInText(text, fadeSpeed);
        for (int i = 0; i < dialog.Length; i++)
        {
          yield return new WaitForSeconds(speed);
          fullDialog = fullDialog + dialog[i];
          text.SetText(fullDialog);
        }
        yield return new WaitForSeconds(waitAfter);
        yield return TextFade.FadeOutText(text, fadeSpeed);
      }
    }

    private TMP_Text GetSpeaker(Speaker speaker)
    {
      switch (speaker)
      {
        case Speaker.Speaker1:
          return speaker1;
        case Speaker.Speaker2:
          return speaker2;
        case Speaker.Speaker3:
          return speaker3;
      }
      return speaker1;
    }

  }
}
