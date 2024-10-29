using UnityEngine;

public class EnemyAnimationSystem: MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    
    private Animator _characterAnimator;
    private AgentMoveSystem _agentMoveSystem;

    private void Awake()
    {
        _characterAnimator = GetComponentInChildren<Animator>();
        _agentMoveSystem = GetComponent<AgentMoveSystem>();
    }

    private void Update()
    {
        _characterAnimator.SetFloat(Speed, _agentMoveSystem.GetSpeedPercent());
    }
}