using System.Collections.Generic;
using UnityEngine;
using RFG;
using RFG.Platformer;

namespace Game
{
  public class HUDHealth : MonoBehaviour
  {
    [Header("Settings")]
    public NewHealthBehaviour HealthBehaviour;
    public int HeartSteps = 5;
    public GameObject[] hearts;

    [HideInInspector]
    private int _maxHearts;
    private List<SpriteSwitcher> _spriteSwitchers;

    private void Awake()
    {
      _spriteSwitchers = new List<SpriteSwitcher>();
      for (int i = 0; i < hearts.Length; i++)
      {
        GameObject heart = hearts[i];
        _spriteSwitchers.Add(heart.GetComponent<SpriteSwitcher>());
      }
    }

    private void Start()
    {
      CalculateMaxHearts(HealthBehaviour.MaxHealth);
    }

    private void CalculateMaxHearts(float maxHealth)
    {
      _maxHearts = (int)(maxHealth / HeartSteps);
      SetMaxHearts(_maxHearts);
    }

    public void SetMaxHearts(int max)
    {
      _maxHearts = max;
      for (int i = 0; i < hearts.Length; i++)
      {
        hearts[i].SetActive(i < _maxHearts);
      }
    }

    public void OnHealthChange(float maxHealth, float currentHealth)
    {
      CalculateMaxHearts(maxHealth);

      float heartHealthStepPercent = (maxHealth / _maxHearts) / 100;
      // 25 / 5 / 100 = .05;

      float currentHealthPercent = currentHealth / maxHealth;
      // 10 / 25 = .4

      float heartHealth = 1;
      for (int i = _maxHearts - 1; i >= 0; i--)
      {
        SpriteSwitcher heart = _spriteSwitchers[i];
        int heartIndex = 0;
        for (int j = 0; j < HeartSteps; j++)
        {
          if (currentHealthPercent < heartHealth)
          {
            heartIndex = j;
            heartHealth -= heartHealthStepPercent;
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

    private void OnEnable()
    {
      HealthBehaviour.OnHealthChange += OnHealthChange;
    }

    private void OnDisable()
    {
      HealthBehaviour.OnHealthChange -= OnHealthChange;
    }

  }
}