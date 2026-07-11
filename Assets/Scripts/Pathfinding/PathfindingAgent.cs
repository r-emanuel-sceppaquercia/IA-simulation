using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAgent : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float angle;
    [field: SerializeField] public LayerMask ObstacleMask { get; private set; }
    [SerializeField] private LayerMask targetMask;

    [field: SerializeField] public PlayerController Target { get; private set; }
    [field: SerializeField] public bool EnemyInSight { get; private set; }

    [SerializeField] private float speed = 5;
    [SerializeField] private Color color;

    [SerializeField] private List<PathNode> patrolRoute;
    public List<PathNode> PatrolRoute => patrolRoute;

    private Heuristic heuristic;

    public bool IsAlerted { get; private set; }
    public PathNode PlayerClosestNode { get; private set; }
    public Vector3 LastPlayerPosition { get; private set; }

    private LineOfSight lineOfSight = new LineOfSight();

    [field: SerializeField] public PathNode CurrentNode { get; set; }
    [field: SerializeField] public PathNode TargetNode { get; set; }

    private void Awake()
    {
        heuristic = new Heuristic();
        CurrentNode = patrolRoute[0];

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.color = color;
        }

        EventManager.SubscribeToEvent(EventType.PLAYER_DETECTED, Alert);

        InvokeRepeating(nameof(UpdateFOV), 0f, 0.2f);
    }

    public bool Satisfies(PathNode current) => current == TargetNode;
    public List<PathNode> GetCurrentNodeNeighbors(PathNode current) => current.GetNeighbors();
    public float GetCost(PathNode current) => current.Cost;
    public float HeuristicCost(PathNode node)
    {
        return heuristic.EuclideanHeuristic(node.transform.position, TargetNode.transform.position);
    }

    private void UpdateFOV()
    {
        var visibleTargets = lineOfSight.FindVisibleTargets(transform, range, angle, targetMask, ObstacleMask);
        var targetInRange = visibleTargets.Count > 0;

        EnemyInSight = targetInRange;

        Debug.Log("EnemyInSight: " + EnemyInSight);

        if (targetInRange)
        {
            Debug.Log("Player is in radius");
            LastPlayerPosition = visibleTargets[0].transform.position;
            PlayerClosestNode = GetClosestNode(LastPlayerPosition);
        }
    }

    public bool TargetInSight(Vector3 targetPosition)
    {
        return lineOfSight.IsInLineOfSightToPoint(transform, targetPosition, ObstacleMask);
    }

    public void Alert(object[] data)
    {
        print("Data type: " + data[0]);

        if ((PathfindingAgent)data[0] == this)
            return;

        print("Agent is alerted: " + ((PathfindingAgent)data[0] == this));

        IsAlerted = true;
        PlayerClosestNode = (PathNode)data[1];
        LastPlayerPosition = (Vector3)data[2];
    }

    public void ClearAlert()
    {
        IsAlerted = false;
    }

    public void Move(Vector3 targetPosition)
    {
        targetPosition.y = transform.position.y;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.forward = Vector3.Lerp(transform.forward, direction, 10f * Time.deltaTime);
    }

    public void MoveDirection(Vector3 direction)
    {
        direction.y = 0;
        transform.position += direction.normalized * speed * Time.deltaTime;
        // modified
        if (direction.sqrMagnitude > 0.3f)
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, 10f * Time.deltaTime);
    }

    public void MoveThroughPathNodes(List<PathNode> path)
    {
        if (path.Count > 0)
        {
            var dir = new Vector3(path[0].transform.position.x, path[0].transform.position.y, path[0].transform.position.z) - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, dir, 10f * Time.deltaTime);

            if (dir.magnitude < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
    }

    private PathNode GetClosestNode(Vector3 position)
    {
        PathNode closest = null;
        float closestDistance = float.MaxValue;

        foreach (var node in patrolRoute)
        {
            float sqrDistance = (node.transform.position - position).sqrMagnitude;

            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                closest = node;
            }
        }

        return closest;
    }

    public PathNode GetClosestNodeToPosition()
    {
        PathNode[] allNodes = FindObjectsByType<PathNode>(FindObjectsSortMode.None);

        PathNode closest = null;
        float closestDistance = float.MaxValue;

        foreach (var node in allNodes)
        {
            float sqrDistance = (node.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance < closestDistance)
            {
                closestDistance = sqrDistance;
                closest = node;
            }
        }

        return closest != null ? closest : patrolRoute[0];
    }

    public void SetMove(List<PathfindingNode> path)
    {
        transform.position = new Vector3(path[0].transform.position.x, path[0].transform.position.y, path[0].transform.position.z - 1);
        StartCoroutine(StartMoving(path));
    }

    public IEnumerator StartMoving(List<PathfindingNode> path)
    {
        while (path.Count > 0)
        {
            var dir = new Vector3(path[0].transform.position.x, path[0].transform.position.y, path[0].transform.position.z - 1) - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;

            if (dir.magnitude < 0.25f)
                path.RemoveAt(0);

            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Radius sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);

        // Field of view
        Gizmos.color = Color.green;
        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * range);
        Gizmos.DrawLine(transform.position, transform.position + right * range);
    }

    private void OnDestroy()
    {
        EventManager.UnsubscribeToEvent(EventType.PLAYER_DETECTED, Alert);
    }
}
