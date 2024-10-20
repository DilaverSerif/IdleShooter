using System;
using _BASE_.Joystick_Pack.Scripts.Base;
using _BASE_.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityHFSM;

namespace _GAME_.Scripts
{
    public enum EPlayerState
    {
        Idle,
        Move,
        Jump,
        Stack,
        Unstack,
        TargetMove,
        TargetIdle
    }
    public class PlayerBrain : MonoBehaviour
    {
        [ShowInInspector]
        private EPlayerState CurrentState
        {
            get
            {
                if (_stateMachine == null)
                {
                    return EPlayerState.Idle;
                }

                return _stateMachine.ActiveStateName;
            }
        }
        private StateMachine<EPlayerState,PlayerBrain> _stateMachine;
        internal Targeting<EnemyHealth> Targeting;
        internal PhysicsBasedCharacterController CharacterController;
        internal PlayerInventory Inventory;
        private void Awake()
        {
            CharacterController = GetComponent<PhysicsBasedCharacterController>();
            Targeting = GetComponent<Targeting<EnemyHealth>>();
            Inventory = GetComponent<PlayerInventory>();
            
            _stateMachine = new StateMachine<EPlayerState,PlayerBrain>();
            var idleState = new IdleState(this); 
            var walkState = new WalkState(this);
            var targetMoveState = new TargetMoveState(this);
            var targetIdleState = new TargetIdleState(this);
            
            _stateMachine.AddState(EPlayerState.Idle, idleState);
            _stateMachine.AddState(EPlayerState.Move, walkState);
            _stateMachine.AddState(EPlayerState.TargetMove, targetMoveState);
            _stateMachine.AddState(EPlayerState.TargetIdle, targetIdleState);
            
            _stateMachine.AddTransitionFromAny(EPlayerState.Idle,ToIdle);
            _stateMachine.AddTransitionFromAny(EPlayerState.Move,ToMove);
            
            _stateMachine.AddTransitionFromAny(EPlayerState.TargetMove,ToTargetMove);
            _stateMachine.AddTransitionFromAny(EPlayerState.TargetIdle,ToTargetIdle);
            
            _stateMachine.SetStartState(EPlayerState.Idle);
            _stateMachine.Init();
        }

        private void Update()
        {
            _stateMachine.OnLogic();
        }

        private bool ToTargetIdle(Transition<EPlayerState> arg)
        {
            return !Joystick.Instance.Moved && Targeting.HasTarget;
        }

        private bool ToTargetMove(Transition<EPlayerState> arg)
        {
            return Joystick.Instance.Moved && Targeting.HasTarget;
        }

        private bool ToMove(Transition<EPlayerState> arg)
        {
            return Joystick.Instance.Moved && !Targeting.HasTarget;
        }

        private bool ToIdle(Transition<EPlayerState> arg)
        {
            return !Joystick.Instance.Moved && !Targeting.HasTarget;
        }
    }
}