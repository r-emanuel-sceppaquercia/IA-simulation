using System.Collections.Generic;
using UnityEngine;

public class AgentAlertState<T> : States<T>
{
    private PathfindingAgent agent;

    [SerializeField] private List<PathNode> currentPath;
    [SerializeField] private PathNode start;

    private FSM<AgentStates> fsm;
    private AStar<PathNode> aStar;

    private ISteering obstacleAvoidance;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    public AgentAlertState(PathfindingAgent agent, FSM<AgentStates> fsm)
    {
        this.agent = agent;
        this.fsm = fsm;
    }

    public override void Awake()
    {
        aStar = new AStar<PathNode>();
        agent.TargetNode = agent.PlayerClosestNode;
        currentPath = aStar.CalculateAStar(agent.CurrentNode, agent.Satisfies, agent.GetCurrentNodeNeighbors, agent.GetCost, agent.HeuristicCost);

        obstacleAvoidance = new ObstacleAvoidance(agent.transform, agent.Target.transform, 3f, 5f, agent.ObstacleMask);
    }

    public override void Execute()
    {
        // if the player last position is in sight we move to that position
        if (agent.TargetInSight(agent.LastPlayerPosition))
        {
            if (!isWaiting)
            {
                Vector3 avoidDir = obstacleAvoidance.GetDir();
                agent.Move(agent.LastPlayerPosition + avoidDir);

                // If we reach the point and the player is not in sight we wait 1 or 2 seconds and return to patrol
                if (Vector3.Distance(agent.transform.position, agent.LastPlayerPosition) < 0.25f)
                {
                    isWaiting = true;
                    waitTimer = 0f;
                }
            }
            else
            {
                // wait 2 seconds and return to patrol
                waitTimer += Time.deltaTime;
                if (waitTimer >= 2f)
                {
                    agent.CurrentNode = agent.GetClosestNodeToPosition();
                    fsm.Transition(AgentStates.PATROL);
                }
            }
        }
        // if not, we move the the closest node
        else
        {
            Debug.Log("Moving to target node");
            agent.MoveThroughPathNodes(currentPath);
        }

        // if the enemy is in sight we transition to chase mode
        if (agent.EnemyInSight)
        {
            fsm.Transition(AgentStates.CHASE);
        }
    }


    public override void Sleep()
    {
        agent.ClearAlert();
        isWaiting = false;
    }
}