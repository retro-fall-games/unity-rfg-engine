using UnityEngine;

namespace RFG
{
  public class CharacterInput : MonoBehaviour
  {
    public Button JumpButton { get; private set; }
    public Button DashButton { get; private set; }
    public Button PauseButton { get; private set; }
    public Button PrimaryFireButton { get; private set; }
    public Button SecondaryFireButton { get; private set; }
    public InputManager InputManager => _inputManager;
    private InputManager _inputManager;

    private void Awake()
    {
      _inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;
    }

    private void Start()
    {
      _inputManager.AddButton(JumpButton = new Button("Jump"));
      _inputManager.AddButton(DashButton = new Button("Dash"));
      _inputManager.AddButton(PauseButton = new Button("Pause"));
      _inputManager.AddButton(PrimaryFireButton = new Button("Fire1"));
      _inputManager.AddButton(SecondaryFireButton = new Button("Fire2"));
    }

  }
}
