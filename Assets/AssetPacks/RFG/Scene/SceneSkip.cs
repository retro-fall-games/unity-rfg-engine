using System.Collections;
using UnityEngine;

namespace RFG
{
  [AddComponentMenu("RFG/Scene/Scene Skip")]
  public class SceneSkip : MonoBehaviour
  {
    public enum SkipType { AutoSkip, SkipOnFirstInput };

    [Header("Settings")]
    public string NextScene;
    public float WaitForSeconds = 3f;
    public SkipType skipType = SceneSkip.SkipType.AutoSkip;

    [HideInInspector]
    private bool _hasSkipped = false;

    private void Start()
    {
      if (skipType == SkipType.AutoSkip)
      {
        StartCoroutine(SkipScene());
      }
    }

    private IEnumerator SkipScene()
    {
      yield return new WaitForSecondsRealtime(WaitForSeconds);
      RFG.SceneManager.Instance.LoadScene(NextScene);
    }

    private void Update()
    {
      if (skipType == SkipType.SkipOnFirstInput)
      {
        if (!_hasSkipped && (Input.anyKey || Input.touchCount > 0))
        {
          _hasSkipped = true;
          RFG.SceneManager.Instance.LoadScene(NextScene);
        }
      }
    }
  }
}