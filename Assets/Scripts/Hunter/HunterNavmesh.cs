using UnityEngine;
using UnityEngine.AI;

public class HunterNavmesh : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] patrolPoints;
    private int currentIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.SetDestination(patrolPoints[currentIndex].position);
    }

    public void Move()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }

    public void StopPatrolling() => agent.isStopped = true;

    public void ResumePatrolling()
    {
        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentIndex].position);
    }
}
