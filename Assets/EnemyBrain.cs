using System;
using _GAME_.Scripts;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityHFSM;

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead,
    Hurt
}

public abstract class EnemyStateBase : StateBase<EnemyState>
{
    public EnemyBrain EnemyBrain;
    public EnemyStateBase(EnemyBrain enemyBrain,bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState)
    {
        EnemyBrain = enemyBrain;
    }
}

public class IdleState : EnemyStateBase
{
    public IdleState(EnemyBrain enemyBrain, bool needsExitTime = false, bool isGhostState = false) : base(enemyBrain, needsExitTime, isGhostState)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        EnemyBrain._agentMoveSystem.MoveTo(EnemyBrain.transform.position);
    }
}

public class ChaseState : EnemyStateBase
{
    public ChaseState(EnemyBrain enemyBrain, bool needsExitTime = false, bool isGhostState = false) : base(enemyBrain, needsExitTime, isGhostState)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        var target = EnemyBrain._targetingSystem.currentTarget.transform;
        EnemyBrain._agentMoveSystem.MoveTo(target.position);
    }
}

public class AttackState : EnemyStateBase
{
    public AttackState(EnemyBrain enemyBrain, bool needsExitTime = false, bool isGhostState = false) : base(enemyBrain, needsExitTime, isGhostState)
    {
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        EnemyBrain._agentMoveSystem.MoveTo(EnemyBrain.transform.position);
        EnemyBrain._weaponAttackSystem.Attack();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        EnemyBrain._weaponAttackSystem.StopAttack();
    }
}

public class HurtState : EnemyStateBase
{
    public HurtState(EnemyBrain enemyBrain, bool needsExitTime = false, bool isGhostState = false) : base(enemyBrain, needsExitTime, isGhostState)
    {
    }
} 

public class DeadState : EnemyStateBase
{
    public DeadState(EnemyBrain enemyBrain, bool needsExitTime = false, bool isGhostState = false) : base(enemyBrain, needsExitTime, isGhostState)
    {
    }
}

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyAnimationSystem))]
[RequireComponent(typeof(AgentMoveSystem))]
[RequireComponent(typeof(EnemyTargeting))]
[RequireComponent(typeof(AttackSystem))]
public class EnemyBrain : MonoBehaviour
{
    private EnemyHealth _healthSystem;
    private EnemyAnimationSystem _animationSystem;
    public AgentMoveSystem _agentMoveSystem;
    public EnemyTargeting _targetingSystem;
    public EnemyWeaponAttackSystem _weaponAttackSystem;
    
    private StateMachine<EnemyState,EnemyBrain> _stateMachine;

    [ShowInInspector]
    private EnemyState CurrentState => _stateMachine?.ActiveStateName ?? EnemyState.Idle;

    private void Awake()
    {
        _targetingSystem = GetComponent<EnemyTargeting>();
        _agentMoveSystem = GetComponent<AgentMoveSystem>();
        _animationSystem = GetComponent<EnemyAnimationSystem>();
        _healthSystem = GetComponent<EnemyHealth>();
        _weaponAttackSystem = GetComponent<EnemyWeaponAttackSystem>();
        
        _stateMachine = new StateMachine<EnemyState,EnemyBrain>();
    }

    private void Start()
    {
        var idleState = new IdleState(this);
        var chaseState = new ChaseState(this);
        var attackState = new AttackState(this);
        var hurtState = new HurtState(this,true);
        var deadState = new DeadState(this);
       
        _stateMachine.AddState(EnemyState.Idle, idleState);
        _stateMachine.AddState(EnemyState.Chase, chaseState);
        _stateMachine.AddState(EnemyState.Attack, attackState);
        _stateMachine.AddState(EnemyState.Hurt, hurtState);
        _stateMachine.AddState(EnemyState.Dead, deadState);
        
        _stateMachine.AddTransition(EnemyState.Idle,EnemyState.Chase,IdleToChase);
        _stateMachine.AddTransition(EnemyState.Chase,EnemyState.Idle,ChaseToIdle);
        
        _stateMachine.AddTransition(EnemyState.Chase,EnemyState.Attack,ChaseToAttack);
        _stateMachine.AddTransition(EnemyState.Attack,EnemyState.Chase,AttackToChase);

        
        _stateMachine.SetStartState(EnemyState.Idle);
        _stateMachine.Init();
    }

    private void Update()
    {
        _targetingSystem.FindTarget();
        _stateMachine.OnLogic();
    }

    public void SetState(EnemyState state)
    {
        _stateMachine.RequestStateChange(state, true);
    }

    private bool AttackToChase(Transition<EnemyState> arg)
    {
        return !_weaponAttackSystem.TargetInRange(_targetingSystem.currentTarget.transform);
    }

    private bool ChaseToAttack(Transition<EnemyState> arg)
    {
        return _weaponAttackSystem.TargetInRange(_targetingSystem.currentTarget.transform);
    }

    private bool IdleToChase(Transition<EnemyState> arg)
    {
        return _targetingSystem.HasTarget && !_weaponAttackSystem.TargetInRange(_targetingSystem.currentTarget.transform);
    }
    
    private bool ChaseToIdle(Transition<EnemyState> arg)
    {
        return !_targetingSystem.HasTarget;
    }
}