using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace RFG
{
  [AddComponentMenu("RFG/Core/Input/Input Action Map To Text")]
  public class InputActionMapToText : MonoBehaviour
  {
    [Header("Input")]
    /// <summary>Input Action to map to text</summary>
    [Tooltip("Input Action to map to text")]
    public InputActionReference InputReference;
    public int Index = 0;

    [HideInInspector]
    private TMP_Text _text;

    private void Awake()
    {
      _text = GetComponent<TMP_Text>();
      _text.SetText("(" + InputReference.action.GetBindingDisplayString(Index));
    }
  }
}