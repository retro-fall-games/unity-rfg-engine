using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG/Scene/Scene Skip")]
  public class SceneSkip : MonoBehaviour
  {
    [Header("Settings")]
    public string NextScene;
    public float WaitForSeconds = 3f;
    public bool CanSkipOnAnyInput = true;

    [HideInInspector]
    private bool _hasSkipped = false;
    private Coroutine _autoSkipCo;

    private void Start()
    {
      _autoSkipCo = StartCoroutine(AutoSkip());
    }

    private IEnumerator AutoSkip()
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      SceneManager.Instance.LoadScene(NextScene);
    }

    private void FixedUpdate()
    {
      if (CanSkipOnAnyInput)
      {
        bool anyInput = InputEx.AnyInput();

        if (!_hasSkipped && anyInput)
        {
          _hasSkipped = true;
          StopCoroutine(_autoSkipCo);
          SceneManager.Instance.LoadScene(NextScene);
        }
      }
    }

  }
}