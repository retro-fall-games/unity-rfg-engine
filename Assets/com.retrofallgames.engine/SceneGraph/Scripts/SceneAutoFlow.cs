using System.Collections;
using UnityEngine;

namespace RFG.SceneGraph
{
  using Core;

  [AddComponentMenu("RFG/Scene/Scene Auto Flow")]
  public class SceneAutoFlow : MonoBehaviour
  {
    public SceneNode SceneNode;
    public float AutoFlowWaitTime = 0f;
    public bool CanAutoFlowOnAnyInput = true;

    [HideInInspector]
    private bool _hasSkipped = false;
    private Coroutine _autoFlowCo;

    private void Start()
    {
      _autoFlowCo = StartCoroutine(AutoFlow());
    }

    private IEnumerator AutoFlow()
    {
      yield return new WaitForSecondsRealtime(AutoFlowWaitTime);
      SceneManager.Instance.LoadScene(SceneNode.SceneName);
    }

    private void FixedUpdate()
    {
      if (!CanAutoFlowOnAnyInput)
        return;

      bool anyInput = InputEx.AnyInput();

      if (!_hasSkipped && anyInput)
      {
        _hasSkipped = true;
        StopCoroutine(_autoFlowCo);
        SceneManager.Instance.LoadScene(SceneNode.SceneName);
      }
    }

  }
}