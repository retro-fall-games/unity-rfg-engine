using System.Collections;
using UnityEngine;

namespace RFG
{
  public class Cutscene : MonoBehaviour
  {
    public UnityEngine.UI.Button skipButton;
    public bool isRunning = false;
    public bool isSkipable = false;
    private bool skip = false;
    private IEnumerator _coroutine;

    protected virtual void Awake()
    {
      if (skipButton != null)
      {
        skipButton.gameObject.SetActive(false);
      }
    }

    private void Update()
    {
      if (skipButton != null && isRunning && isSkipable && !skip)
      {
        if (Input.GetMouseButtonDown(0))
        {
          skipButton.gameObject.SetActive(true);
        }
      }
    }

    public void SetCoroutine(IEnumerator coroutine)
    {
      _coroutine = coroutine;
    }

    public void Run()
    {
      if (_coroutine != null)
      {
        StartCoroutine(RunCo());
      }
    }

    public IEnumerator RunCo()
    {
      isRunning = true;
      yield return StartCoroutine(_coroutine);
      isRunning = false;
    }

    public void Skip(float waitAfter = 0f)
    {
      skip = true;
      isRunning = false;
      skipButton.gameObject.SetActive(false);
      StopCoroutine(_coroutine);
      OnSkipEnter();
      StartCoroutine(SkipCo(waitAfter));
    }

    private IEnumerator SkipCo(float waitAfter = 0f)
    {
      yield return new WaitForSeconds(waitAfter);
      OnSkipExit();
    }

    protected virtual void OnSkipEnter()
    {
    }
    protected virtual void OnSkipExit()
    {
    }

  }
}