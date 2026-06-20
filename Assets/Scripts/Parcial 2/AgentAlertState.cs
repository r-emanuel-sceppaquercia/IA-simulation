using System.Collections.Generic;
using UnityEngine;

public class AgentAlertState<T> : States<T>
{
    private PathfindingAgent agent;

    [SerializeField] private List<PathNode> currentPath;
    [SerializeField] private PathNode start;

    private FSM<AgentStates> fsm;

    private AStar<PathNode> aStar;

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
    }

    public override void Execute()
    {
        // if the enemy is in sight we transition to chase mode
        if (agent.EnemyInSight)
        {
            Debug.Log("Transition to chase mode");
            fsm.Transition(AgentStates.CHASE);
        }

        // if the player last position is in sight we move to that position
        if (agent.TargetInSight(agent.LastPlayerPosition))
        {
            // TODO: obstacle avoidance
            agent.Move(agent.LastPlayerPosition);

            // If we reach the point and the player is not in sight we wait 1 or 2 seconds and return to patrol
            if (Vector3.Distance(agent.transform.position, agent.LastPlayerPosition) < 0.25f)
            {
                // TODO: wait seconds and return to patrol
                fsm.Transition(AgentStates.PATROL);
            }
        }
        // if not, we move the the closest node
        else
        {
            Debug.Log("Moving to target node");
            agent.MoveThroughPathNodes(currentPath);
        }
    }

    public override void Sleep()
    {
        agent.ClearAlert();

        var currentNode = agent.TargetNode;
        agent.TargetNode = agent.CurrentNode;
        agent.CurrentNode = currentNode;
    }
}
