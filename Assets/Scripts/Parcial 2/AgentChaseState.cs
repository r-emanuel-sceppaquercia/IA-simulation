using UnityEngine;

public class AgentChaseState<T> : States<T>
{
    private PathfindingAgent agent;
    private Transform target;
    private ISteering steering;
    private ISteering obstacleAvoidance;

    private FSM<AgentStates> fsm;

    private float waitTimer = 0f;

    public AgentChaseState(PathfindingAgent agent, FSM<AgentStates> fsm)
    {
        this.agent = agent;
        this.fsm = fsm;
    }

    public override void Awake()
    {
        steering = new Seek(agent.transform, agent.Target.transform);
        obstacleAvoidance = new ObstacleAvoidance(agent.transform, agent.Target.transform, 3f, 5f, agent.ObstacleMask);
    }

    public override void Execute()
    {
        // While agent.EnemyInSight -> chase
        if (agent.EnemyInSight)
        {
            waitTimer = 0f;

            // seek and avoid
            agent.MoveDirection(steering.GetDir() + obstacleAvoidance.GetDir());
        }
        else
        {
            // if not in sight wait 3 to 5 seconds and return to patrol
            waitTimer += Time.deltaTime;

            if (waitTimer >= 3f)
            {
                fsm.Transition(AgentStates.PATROL);
            }
        }
    }

    public override void Sleep()
    {
        waitTimer = 0f;

        agent.CurrentNode = agent.GetClosestNodeToPosition();
    }
}