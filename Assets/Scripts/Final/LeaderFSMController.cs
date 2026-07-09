using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum LeaderStates
{
    PATROL,
    COMBAT,
    REST
}

[RequireComponent(typeof(EntityStats))]
public class LeaderFSMController : MonoBehaviour
{
    private FSM<LeaderStates> _fsm;
    public EntityStats stats { get; private set; }

    [Header("Radar / LineOfSight")]
    public LineOfSight lineOfSight = new LineOfSight();
    public float sightRange = 15f;
    public float sightAngle = 90f;
    public LayerMask enemyLayer; // Layer for the Leader/Agents B
    public LayerMask obstacleLayer;

    [Header("Pathfinding")]
    public float speed = 5f;
    public List<PathNode> patrolRoute;
    public PathNode CurrentNode { get; set; }

    public PathNode TargetNode { get; set; }


    // To be read by the agents
    public Transform Target { get; set; } // Enemy that has been detected
    public bool EnemyInSight => Target != null;

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        if (patrolRoute.Count > 0) CurrentNode = patrolRoute[0];

        SetUpFSM();
    }

    private void SetUpFSM()
    {
        _fsm = new FSM<LeaderStates>();

        // estados
        // var patrolState = new LeaderPatrolState(this, _fsm);
        // var combatState = new LeaderCombatState(this, _fsm);
        // var restState = new LeaderRestState(this, _fsm);

        // TODO: Agregar transiciones
        // _fsm.SetInit(patrolState);
    }

    private void Update()
    {
        // _fsm.OnUpdate();

        DetectEnemies();
    }

    private void DetectEnemies()
    {
        var visibleTargets = lineOfSight.FindVisibleTargets(transform, sightRange, sightAngle, enemyLayer, obstacleLayer);

        if (visibleTargets.Count > 0)
        {
            Transform closestTarget = null;
            float minDistance = float.MaxValue;

            // for every enemy
            foreach (Transform potentialTarget in visibleTargets)
            {
                // distance between leader and enemy
                float distance = Vector3.Distance(transform.position, potentialTarget.position);

                // shorter distance is what we go for
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = potentialTarget;
                }
            }

            Target = closestTarget;
        }
        else
        {
            Target = null; // we see nothing
        }
    }

    // I'm not sure ---------------------
    // A* Delegates (Pathfinding)
    public bool Satisfies(PathNode current) => current == TargetNode;
    public List<PathNode> GetCurrentNodeNeighbors(PathNode current) => current.GetNeighbors();
    public float GetCost(PathNode parent, PathNode child) => 1f;
    public float Heuristic(PathNode current)
    {
        if (TargetNode == null) return 0f;
        return Vector3.Distance(current.transform.position, TargetNode.transform.position);
    }
}