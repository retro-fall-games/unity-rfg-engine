namespace RFG.Platformer
{
  using BehaviourTree;
  using Navigation;

  public class MovementPathActionNode : ActionNode
  {
    protected override void OnStart()
    {
      AIBrainBehaviour brain = context as AIBrainBehaviour;
      brain.Context.characterContext.character.MovementState.ChangeState(typeof(WalkingState));
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
      AIBrainBehaviour brain = context as AIBrainBehaviour;
      brain.Context.controller.RotateTowards(brain.Context.movementPath.NextPath);
      if (!brain.Context.movementPath.autoMove)
      {
        brain.Context.movementPath.Move();
        brain.Context.movementPath.CheckPath();
      }

      if (brain.Context.movementPath.state == MovementPath.State.OneWay && brain.Context.movementPath.ReachedEnd)
      {
        brain.Context.characterContext.character.MovementState.ChangeState(typeof(IdleState));
        return State.Success;
      }
      return State.Running;
    }
  }
}