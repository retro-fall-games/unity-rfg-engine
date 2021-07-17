using System.Collections;
using UnityEngine;


namespace RFG
{
  [AddComponentMenu("RFG Platformer/Character/Behaviour/Wall Jump Behaviour")]
  public class WallJumpBehaviour : CharacterBehaviour
  {
    public Vector2 wallJumpForce = new Vector2(10f, 4f);
    public bool shouldReduceNumberOfJumpsLeft = true;
    private JumpBehaviour _jumpBehaviour;
    private Button _jumpButton;

    public override void InitBehaviour()
    {
      _jumpBehaviour = _character.FindBehaviour<JumpBehaviour>();
      StartCoroutine(InitBehaviourCo());
    }

    private IEnumerator InitBehaviourCo()
    {
      yield return new WaitUntil(() => _character.CharacterInput != null);
      yield return new WaitUntil(() => _character.CharacterInput.JumpButton != null);
      _jumpButton = _character.CharacterInput.JumpButton;
      _jumpButton.State.OnStateChange += JumpButtonOnStateChanged;
    }

    private void JumpButtonOnStateChanged(ButtonStates state)
    {
      switch (state)
      {
        case ButtonStates.Down:
          if (_character.MovementState.CurrentState == MovementStates.WallClinging)
          {
            WallJump();
          }
          break;
      }
    }

    private void WallJump()
    {
      float wallJumpDirection;

      _character.MovementState.ChangeState(MovementStates.WallJumping);

      if (_jumpBehaviour != null && shouldReduceNumberOfJumpsLeft)
      {
        _jumpBehaviour.SetNumberOfJumpsLeft(_jumpBehaviour.NumberOfJumpsLeft - 1);
      }

      _character.Controller.SlowFall(0f);

      if (_character.Controller.State.IsFacingRight)
      {
        wallJumpDirection = -1f;
      }
      else
      {
        wallJumpDirection = 1f;
      }

      Vector2 wallJumpVector = new Vector2(wallJumpDirection * wallJumpForce.x, Mathf.Sqrt(2f * wallJumpForce.y * Mathf.Abs(_character.Controller.Parameters.gravity)));

      _character.Controller.AddForce(wallJumpVector);
    }

  }
}