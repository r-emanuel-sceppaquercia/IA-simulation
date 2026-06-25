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

        LayerMask mask = LayerMask.GetMask("Obstacle");
        obstacleAvoidance = new ObstacleAvoidance(agent.transform, agent.Target.transform, 3f, 5f, mask);
    }

    public override void Execute()
    {
        // While agent.EnemyInSight -> chase
        if (agent.EnemyInSight)
        {
            waitTimer = 0f;

            // TODO: obstacle avoidance resuelto --
            Vector3 seekDir = steering.GetDir();
            Vector3 avoidDir = obstacleAvoidance.GetDir();

            // seeks and avoids
            agent.MoveDirection((seekDir + avoidDir).normalized);
        }
        else
        {
            // TODO: if not in sight wait 3 to 5 seconds and return to patrol resuelto
            agent.MoveDirection(Vector3.zero); // we stop em
            waitTimer += Time.deltaTime;

            if (waitTimer >= 3f) // waits 3 secs
            {
                fsm.Transition(AgentStates.PATROL);
            }
        }
    }

    public override void Sleep()
    {
        agent.Move(Vector3.zero);
        waitTimer = 0f;
        //test TEST
        agent.CurrentNode = agent.GetClosestNodeToPosition();
    }
}