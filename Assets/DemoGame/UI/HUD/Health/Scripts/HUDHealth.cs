using System.Collections.Generic;
using UnityEngine;
using RFG;
using RFG.Platformer;

namespace Game
{
  public class HUDHealth : MonoBehaviour
  {
    [Header("Settings")]
    public FloatReference HealthReference;
    public FloatReference MaxHealthReference;
    public int HeartSteps = 5;
    public GameObject[] hearts;

    [HideInInspector]
    private int _maxHearts;
    private float _currentHealth;
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
      CalculateMaxHearts();
    }

    private void LateUpdate()
    {
      if (_currentHealth != HealthReference.Value)
      {
        _currentHealth = HealthReference.Value;
        UpdateUI();
      }
    }

    private void CalculateMaxHearts()
    {
      _maxHearts = (int)(MaxHealthReference.Value / HeartSteps);
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

    public void UpdateUI()
    {
      CalculateMaxHearts();

      int startingIndex = 0;
      for (int i = 0; i < _maxHearts; i++)
      {
        SpriteSwitcher heart = _spriteSwitchers[i];
        int heartIndex = 0;
        int endingIndex = startingIndex + HeartSteps + 1;
        for (int j = startingIndex; j < endingIndex; j++)
        {
          startingIndex++;
          if (_currentHealth > j)
          {
            heartIndex++;
            continue;
          }
          else
          {
            break;
          }
        }
        startingIndex--;
        if (heartIndex > HeartSteps)
        {
          heartIndex = HeartSteps;
        }
        heart.SetImageAtIndex(heartIndex);
      }
    }

  }
}