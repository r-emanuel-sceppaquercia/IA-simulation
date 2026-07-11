using System.Collections.Generic;
using UnityEngine;

public class LeaderFleeState<T> : States<T>
{
    private LeaderController leaderController;
    private FSM<LeaderState> fsm;

    private ThetaStar thetaStar;
    private List<PathNode> currentPath;

    private Seek seek;
    private Vector3 hideoutPosition;

    public LeaderFleeState(FSM<LeaderState> fsm, LeaderController leaderController)
    {
        this.fsm = fsm;
        this.leaderController = leaderController;

        thetaStar = new ThetaStar();
    }

    public override void Awake()
    {
        leaderController.View.ChangeColor(Color.blue);

        hideoutPosition = NodeManager.Instance.GetHideoutPosition(leaderController.Model.HerdType);

        PathNode startNode = NodeManager.Instance.GetClosestNodeToPosition(leaderController.transform.position);
        PathNode endNode = NodeManager.Instance.GetClosestNodeToHideout(leaderController.Model.HerdType);

        currentPath = thetaStar.CalculateThetaStar(startNode, endNode, HeuristicType.Euclidean, leaderController.LineOfSight.InSight);

        seek = new Seek(leaderController.transform, hideoutPosition);

        PrintCurrentPath(Color.blue);
    }

    public override void Execute()
    {
        // If target in sight
        if (leaderController.LineOfSight.IsInSight(leaderController.transform.position, hideoutPosition))
        {
            Debug.Log("In range of hideout position");
            leaderController.Movement.MoveDirection(seek.GetDir(), leaderController.Model.FleeSpeed);

            if (Vector3.Distance(leaderController.transform.position, hideoutPosition) < 0.3f)
            {
                fsm.Transition(LeaderState.ROAMING);
                return;
            }
        }
        else
        {
            leaderController.Movement.MoveThroughPathNodes(currentPath, leaderController.Model.FleeSpeed, leaderController.LineOfSight.IsInSight);
        }
    }

    public override void Sleep()
    {

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
