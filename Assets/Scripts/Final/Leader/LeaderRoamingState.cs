using System.Collections.Generic;
using UnityEngine;

public class LeaderRoamingState<T> : States<T>
{
    private FSM<LeaderState> fsm;
    private LeaderController leaderController;

    private ThetaStar thetaStar;
    private List<PathNode> currentPath;
    private PathNode currentTargetNode;

    public LeaderRoamingState(FSM<LeaderState> fsm, LeaderController leaderController)
    {
        this.fsm = fsm;
        this.leaderController = leaderController;

        thetaStar = new ThetaStar();
    }

    public override void Awake()
    {
        leaderController.View.ChangeColor(Color.yellow);

        CalculateRandomPath();
    }

    public override void Execute()
    {
        // If enemy in range transition to combat state
        if (leaderController.LineOfSight.EnemiesInRange())
        {
            fsm.Transition(LeaderState.COMBAT);
        }

        // If leaderController.HasMovingPoint
        if (leaderController.HasMovingPoint())
        {
            fsm.Transition(LeaderState.MOVING);
        }

        // Else move to random node
        MoveThroughPath();
    }

    public override void Sleep()
    {

    }

    private void CalculateRandomPath()
    {
        PrintCurrentPath(Color.grey);

        currentTargetNode = NodeManager.Instance.GetRandomNode();
        PathNode startNode = NodeManager.Instance.GetClosestNodeToPosition(leaderController.transform.position);

        currentPath = thetaStar.CalculateThetaStar(startNode, currentTargetNode, HeuristicType.Euclidean, leaderController.LineOfSight.InSight);

        PrintCurrentPath(Color.yellow);
    }

    private void MoveThroughPath()
    {
        if (currentPath.Count == 0)
            CalculateRandomPath();

        // Navigate through the path
        leaderController.Movement.MoveThroughPathNodes(currentPath, leaderController.Model.RoamingSpeed, leaderController.LineOfSight.IsInSight);

        // Last node reached -> recalculate a random path
        if (Vector3.Distance(leaderController.transform.position, currentTargetNode.transform.position) < 0.3f)
            CalculateRandomPath();
    }

    private void PrintCurrentPath(Color color)
    {
        if (currentPath == null) return;

        foreach (var item in currentPath)
        {
            item.GetComponentInChildren<Renderer>().material.color = color;
        }
    }
}
