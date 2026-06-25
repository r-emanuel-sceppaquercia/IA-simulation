using System.Collections.Generic;
using UnityEngine;

public class AgentPatrolState<T> : States<T>
{
    private PathfindingAgent agent;

    // current path calculated by the AStar algorithm
    private List<PathNode> currentPath;

    // patrol index points to the current patrol point
    private int patrolIndex;
    // current path index points to the next pathNode in the currentPath (calculated by AStar)
    private int currentPathIndex;

    private AStar<PathNode> aStar;

    FSM<AgentStates> fsm;

    public AgentPatrolState(PathfindingAgent agent, FSM<AgentStates> fsm)
    {
        this.agent = agent;
        this.fsm = fsm;

        aStar = new AStar<PathNode>();
    }

    public override void Awake()
    {
        patrolIndex = 0;
        currentPathIndex = 0;

        agent.TargetNode = agent.PatrolRoute[patrolIndex];

        currentPath = aStar.CalculateAStar(agent.CurrentNode, agent.Satisfies, agent.GetCurrentNodeNeighbors, agent.GetCost, agent.Heuristic);
    }

    public override void Execute()
    {
        // If the enemy is in sight we transition to the Chase state
        EnemyInSight();

        // If another agent detects the player we transition to the Alert state
        if (agent.IsAlerted)
        {
            fsm.Transition(AgentStates.ALERT);
        }

        // Every frame the agent move closer to the nextNode in the currentPath list
        PathNode nextNode = currentPath[currentPathIndex];
        agent.Move(nextNode.transform.position);

        // If we reach the current node we set the current node as the next and increase the pathIndex
        // to point to the next node in the pathNode list (AStar path)
        if (Vector3.Distance(agent.transform.position, nextNode.transform.position) < 0.5f)
        {
            agent.CurrentNode = nextNode;
            currentPathIndex++;
        }

        // If we complete the current path (that means we reach the patrol point) we point the the next patrol node
        if (currentPathIndex >= currentPath.Count)
        {
            patrolIndex++;

            // If we reach the last patrol node we restart from the begining
            if (patrolIndex >= agent.PatrolRoute.Count)
                patrolIndex = 0;

            agent.TargetNode = agent.PatrolRoute[patrolIndex];

            // We recalculate the path to the next node and restart the path index to match the new current path
            currentPath = aStar.CalculateAStar(agent.CurrentNode, agent.Satisfies, agent.GetCurrentNodeNeighbors, agent.GetCost, agent.Heuristic);
            currentPathIndex = 0;
        }
    }

    public override void Sleep() { }

    private void EnemyInSight()
    {
        if (agent.EnemyInSight)
        {
            fsm.Transition(AgentStates.CHASE);

            // We trigger an event that sends to the agents the player position and the closest node
            EventManager.TriggerEvent(EventType.PLAYER_DETECTED, agent, agent.PlayerClosestNode, agent.LastPlayerPosition);
        }
    }
}
