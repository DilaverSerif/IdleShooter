using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentMoveSystem : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public float GetSpeedPercent()
    {
        return _agent.velocity.magnitude / _agent.speed;
    }

    public void AgentTurn(bool turn = true)
    {
        _agent.updateRotation = turn;
    }
    
    public bool AgentTurnTo(Vector3 position)
    {
        var direction = (position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
        return TurnToTarget(position);
    }

    bool TurnToTarget(Vector3 position)
    {
        var direction = (position - transform.position).normalized;
        var angle = Vector3.Angle(transform.forward, direction);
        
        return angle < 5f;
    }
}