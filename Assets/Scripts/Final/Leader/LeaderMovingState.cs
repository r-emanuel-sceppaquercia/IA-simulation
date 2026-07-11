using System.Collections.Generic;
using UnityEngine;

public class LeaderMovingState<T> : States<T>
{
    private LeaderController leaderController;

    private FSM<LeaderState> fsm;

    private ThetaStar thetaStar;
    private List<PathNode> currentPath;

    private Seek seek;
    private ObstacleAvoidance obstacleAvoidance;

    private Vector3 previousTargetPoint;

    public LeaderMovingState(FSM<LeaderState> fsm, LeaderController leaderController)
    {
        this.fsm = fsm;
        this.leaderController = leaderController;

        thetaStar = new ThetaStar();
    }

    public override void Awake()
    {
        leaderController.View.ChangeColor(Color.green);

        CalculatePath();
    }

    public override void Execute()
    {
        // If enemy in range transition to combat state
        if (leaderController.LineOfSight.EnemiesInRange())
        {
            fsm.Transition(LeaderState.COMBAT);
            return;
        }

        // If the target point is updated we recalculate the path
        if (previousTargetPoint != leaderController.targetPoint)
        {
            PrintCurrentPath(Color.grey);
            CalculatePath();
        }

        MoveToPoint();
    }

    public override void Sleep()
    {
        leaderController.SetTargetPoint(Vector3.zero);
    }

    private void PrintCurrentPath(Color color)
    {
        foreach (var item in currentPath)
        {
            item.GetComponentInChildren<Renderer>().material.color = color;
        }
    }

    private void CalculatePath()
    {
        previousTargetPoint = leaderController.targetPoint;

        seek = new Seek(leaderController.transform, leaderController.targetPoint);
        obstacleAvoidance = new ObstacleAvoidance(
            leaderController.transform,
            leaderController.targetPoint,
            leaderController.Model.AvoidRadius,
            leaderController.Model.AvoidWeight,
            leaderController.LineOfSight.GetObstacleMask());

        PathNode startNode = NodeManager.Instance.GetClosestNodeToPosition(leaderController.transform.position);
        PathNode endNode = NodeManager.Instance.GetClosestNodeToPosition(leaderController.targetPoint);

        currentPath = thetaStar.CalculateThetaStar(startNode, endNode, HeuristicType.Euclidean, leaderController.LineOfSight.InSight);

        PrintCurrentPath(Color.red);
    }

    private void MoveToPoint()
    {
        // If target in sight
        if (leaderController.LineOfSight.IsInSight(leaderController.transform.position, leaderController.targetPoint))
        {
            UpdateSteerings(leaderController.targetPoint);

            // Upon reach targetPoint transition to roaming
            if (ReachTarget(leaderController.transform.position, leaderController.targetPoint))
            {
                fsm.Transition(LeaderState.ROAMING);
                return;
            }

            leaderController.Movement.MoveDirection((seek.GetDir() + obstacleAvoidance.GetDir()).normalized, leaderController.Model.Speed);
        }
        else
        {
            // Else we move though the path nodes
            UpdateCurrentNode();

            if (currentPath.Count == 0)
            {
                Debug.Log("Last node reached and still no line of sight of target");
                fsm.Transition(LeaderState.ROAMING);
                return;
            }

            Vector3 currentTarget = currentPath[0].transform.position;

            seek.SetTarget(currentTarget);
            obstacleAvoidance.SetTarget(currentTarget);

            leaderController.Movement.MoveDirection((seek.GetDir() + obstacleAvoidance.GetDir()).normalized, leaderController.Model.Speed);

            if (Vector3.Distance(leaderController.transform.position, currentTarget) < 0.1f)
            {
                currentPath.RemoveAt(0);
            }
        }
    }

    private bool ReachTarget(Vector3 from, Vector3 target)
    {
        from.y = 0;
        target.y = 0;

        return Vector3.Distance(from, target) < 0.3f;
    }

    private void UpdateCurrentNode()
    {
        while (currentPath.Count > 1 && leaderController.LineOfSight.IsInSight(leaderController.transform.position, currentPath[1].transform.position))
        {
            currentPath.RemoveAt(0);
        }
    }

    private void UpdateSteerings(Vector3 target)
    {
        seek.SetTarget(target);
        obstacleAvoidance.SetTarget(target);
    }
}
