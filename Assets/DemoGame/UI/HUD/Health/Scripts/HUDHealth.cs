using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RFG;

namespace Game
{
  public class HUDHealth : MonoBehaviour
  {
    [Header("Settings")]
    public int maxHearts = 3;
    public int heartSteps = 5;
    public GameObject[] hearts;

    [HideInInspector]
    private List<SpriteSwitcher> _spriteSwitchers;

    private void Awake()
    {
      _spriteSwitchers = new List<SpriteSwitcher>();
      for (int i = 0; i < hearts.Length; i++)
      {
        GameObject heart = hearts[i];
        _spriteSwitchers.Add(heart.GetComponent<SpriteSwitcher>());
      }
      SetMaxHearts(maxHearts);
    }

    private void Start()
    {
      StartCoroutine(StartCo());
    }

    private IEnumerator StartCo()
    {
      yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      HealthBehaviour health = player.gameObject.GetComponent<HealthBehaviour>();
      health.OnHealthChange += OnHealthChange;
    }

    public void SetMaxHearts(int max)
    {
      maxHearts = max;
      for (int i = 0; i < hearts.Length; i++)
      {
        GameObject heart = hearts[i];
        if (i >= maxHearts)
        {
          heart.SetActive(false);
        }
      }
    }

    public void OnHealthChange(float maxHealth, float currentHealth)
    {
      float heartHealthPercent = maxHealth / maxHearts / 100;
      // 100 / 3 / 100 = .33;

      float heartStepPercent = (1f / heartSteps) * heartHealthPercent;
      // 1 / 5 = (.2) * .33 = .06

      float currentHealthPercent = currentHealth / maxHealth;
      // 75 / 100 = .75

      float heartHealth = 1;
      for (int i = maxHearts - 1; i >= 0; i--)
      {
        SpriteSwitcher heart = _spriteSwitchers[i];
        int heartIndex = 0;
        for (int j = 0; j < heartSteps; j++)
        {
          if (currentHealthPercent < heartHealth)
          {
            heartIndex = j;
            heartHealth -= heartStepPercent;
            continue;
          }
          else
          {
            break;
          }
        }
        heart.SetImageAtIndex(heartIndex);
      }
    }

  }
}