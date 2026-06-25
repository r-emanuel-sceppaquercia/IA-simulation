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
        currentPath = aStar.CalculateAStar(agent.CurrentNode, agent.Satisfies, agent.GetCurrentNodeNeighbors, agent.GetCost, agent.Heuristic);

        LayerMask mask = LayerMask.GetMask("Obstacle");
        obstacleAvoidance = new ObstacleAvoidance(agent.transform, agent.Target.transform, 3f, 5f, mask);
    }

    public override void Execute()
    {
        // if the enemy is in sight we transition to chase mode
        if (agent.EnemyInSight)
        {
            Debug.Log("Transition to chase mode");
            fsm.Transition(AgentStates.CHASE);
            return;
        }

        // if the player last position is in sight we move to that position
        if (agent.TargetInSight(agent.LastPlayerPosition))
        {
            if (!isWaiting)
            {
                // TODO: obstacle avoidance resuelto
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
                // TODO: wait seconds and return to patrol resuelto --
                waitTimer += Time.deltaTime;
                if (waitTimer >= 2f) // waits 2
                {
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
    }


    //test TEST
    public override void Sleep()
    {
        agent.ClearAlert();
        isWaiting = false;

        //var currentNode = agent.TargetNode;
        //agent.TargetNode = agent.CurrentNode;
        //agent.CurrentNode = currentNode;
        agent.CurrentNode = agent.GetClosestNodeToPosition();
    }
}