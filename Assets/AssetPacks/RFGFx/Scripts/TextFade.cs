using System.Collections;
using UnityEngine;
using TMPro;

namespace RFGFx
{
  public class TextFade : MonoBehaviour
  {
    public static IEnumerator FadeInText(TMP_Text text, float timeSpeed)
    {
      text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
      while (text.color.a < 1.0f)
      {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * timeSpeed));
        yield return null;
      }
    }
    public static IEnumerator FadeOutText(TMP_Text text, float timeSpeed)
    {
      text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
      while (text.color.a > 0.0f)
      {
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * timeSpeed));
        yield return null;
      }
    }

  }
}