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
}