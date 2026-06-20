using UnityEngine;

public class AgentChaseState<T> : States<T>
{
    private PathfindingAgent agent;
    private Transform target;
    private ISteering steering;

    private FSM<AgentStates> fsm;

    public AgentChaseState(PathfindingAgent agent, FSM<AgentStates> fsm)
    {
        this.agent = agent;
        this.fsm = fsm;
    }

    public override void Awake()
    {
        steering = new Seek(agent.transform, agent.Target.transform);
    }

    public override void Execute()
    {
        // While agent.EnemyInSight -> chase
        if (agent.EnemyInSight)
        {
            // TODO: obstacle avoidance
            agent.MoveDirection(steering.GetDir());
        }
        else
        {
            // TODO: if not in sight wait 3 to 5 seconds and return to patrol
            fsm.Transition(AgentStates.PATROL);
        }
    }

    public override void Sleep()
    {
        agent.Move(Vector3.zero);
    }
}
