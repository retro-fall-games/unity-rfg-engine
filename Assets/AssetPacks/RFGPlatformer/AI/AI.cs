using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFG
{
  public class AI : Character
  {
    public TickStateMachine StateMachine => _stateMachine;
    private Aggro _aggro;
    private HealthBehaviour _healthBehaviour;
    private Dictionary<string, System.Action<Collider2D>> _collisionActions = new Dictionary<string, System.Action<Collider2D>>();

    protected TickStateMachine _stateMachine;


    protected override void Awake()
    {
      base.Awake();

      _stateMachine = GetComponent<TickStateMachine>();
      _aggro = GetComponent<Aggro>();
      _healthBehaviour = FindBehaviour<HealthBehaviour>();

      // Setup events
      Controller.onControllerCollidedEvent += onControllerCollider;
      Controller.onTriggerEnterEvent += onTriggerEnterEvent;
      Controller.onTriggerExitEvent += onTriggerExitEvent;
      _healthBehaviour.OnKill += OnKill;
      _aggro.OnAggroChange += OnAggroChange;

      // Register any collision tag events here:
      _collisionActions.Add("Player", PlayerCollision);

      // Initialize State Machine
      _stateMachine = GetComponent<TickStateMachine>();
      var states = new Dictionary<Type, TickBaseState>()
      {
        { typeof(AIIdleState), new AIIdleState(this)},
        { typeof(AIWanderState), new AIWanderState(this)},
        { typeof(AIAggroState), new AIAggroState(this, _aggro)},
      };
      _stateMachine.SetStates(states);
      _stateMachine.OnStateChange += OnTickStateChanged;

    }

    private void onControllerCollider(RaycastHit2D hit)
    {
      // bail out on plain old ground hits cause they arent very interesting
      if (hit.normal.y == 1f)
        return;

      // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
      //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }

    private void onTriggerEnterEvent(Collider2D col)
    {
      if (_collisionActions.ContainsKey(col.gameObject.tag))
      {
        System.Action<Collider2D> func = _collisionActions[col.gameObject.tag];
        if (func != null)
        {
          func(col);
        }
      }
    }

    private void onTriggerExitEvent(Collider2D col)
    {
      // Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    private void PlayerCollision(Collider2D col)
    {
      Knockback knockback = col.gameObject.GetComponent<Knockback>();
      if (knockback == null)
      {
        return;
      }
      Vector2 knockbackVelocity = knockback.GetKnockbackVelocity(transform.position, col.transform.position);
      _healthBehaviour.TakeDamage(knockback.damage, knockback.velocity);
    }

    private void OnKill()
    {
      base.Kill();
      StartCoroutine(KillCo());
    }

    private IEnumerator KillCo()
    {
      yield return new WaitForSeconds(2f);
      Destroy(gameObject);
    }

    private void OnTickStateChanged(TickBaseState state)
    {
      // Debug.Log(state.GetType());
    }

    private void OnAggroChange(bool hasAggro)
    {
      if (hasAggro)
      {
        _stateMachine.SwitchToNewState(typeof(AIAggroState));
      }
      else
      {
        _stateMachine.SwitchToDefaultState();
      }
    }

  }
}