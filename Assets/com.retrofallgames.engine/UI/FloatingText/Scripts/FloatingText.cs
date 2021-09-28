using System.Collections;
using UnityEngine;
using TMPro;

namespace RFG.UI
{
  using Core;

  public class FloatingText : MonoBehaviour, IPooledObject
  {
    [Header("Settings")]
    public TMP_Text text;
    public Vector3 speed = Vector2.zero;
    public float lifetime = 3f;
    public float fadeSpeed = 1f;

    [Header("Target")]
    public Transform target;
    public bool targetIsPlayer = false;

    [HideInInspector]
    private float _timeElapsed = 0f;

    public void OnObjectSpawn(params object[] objects)
    {
      if (objects.Length > 0)
      {
        text.SetText((string)objects[0]);
      }
      _timeElapsed = 0f;
      TextFade.SetAlpha(text, 1);
      if (targetIsPlayer)
      {
        StartCoroutine(WaitForPlayer());
      }
      else
      {
        transform.position = target.position;
      }
    }

    private IEnumerator WaitForPlayer()
    {
      yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player != null)
      {
        target = player.transform;
        transform.position = target.position;
      }
    }

    private void Update()
    {
      _timeElapsed += Time.deltaTime;
      if (_timeElapsed >= lifetime)
      {
        StartCoroutine(FadeOut());
      }
      transform.position += speed * Time.deltaTime;
    }

    private IEnumerator FadeOut()
    {
      yield return TextFade.FadeOutText(text, fadeSpeed);
      gameObject.SetActive(false);
    }
  }
}