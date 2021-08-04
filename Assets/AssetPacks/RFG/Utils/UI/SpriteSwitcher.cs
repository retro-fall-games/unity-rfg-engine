using UnityEngine;
using UnityEngine.UI;

namespace RFG
{
  public class SpriteSwitcher : MonoBehaviour
  {
    [Header("Settings")]
    public Sprite[] sprites;
    public int startingIndex = 0;

    private Image _image;

    private void Awake()
    {
      _image = GetComponent<Image>();
      if (startingIndex != 0)
      {
        SetImageAtIndex(startingIndex);
      }
    }

    public void SetImageAtIndex(int index)
    {
      _image.sprite = sprites[index];
    }
  }
}