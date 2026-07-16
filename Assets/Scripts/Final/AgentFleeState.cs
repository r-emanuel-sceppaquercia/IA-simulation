using System.Collections.Generic;
using UnityEngine;

public class AgentFleeState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;

    private ThetaStar thetaStar;
    private List<PathNode> currentPath;
    private PathNode currentTargetNode;

    private Seek seek;

    public AgentFleeState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;

        thetaStar = new ThetaStar();
    }

    public override void Awake()
    {
        _controller.View.ChangeColor(Color.blue);

        PathNode startNode = NodeManager.Instance.GetClosestNodeToPosition(_controller.transform.position);
        PathNode endNode = NodeManager.Instance.GetClosestNodeToHideout(_controller.MyLeader.Model.HerdType);

        currentPath = thetaStar.CalculateThetaStar(startNode, endNode, HeuristicType.Euclidean, _controller.LineOfSight.InSight);

        seek = new Seek(_controller.transform, _controller.Hideout.transform.position);
    }

    public override void Execute()
    {
        // If target in sight
        if (_controller.LineOfSight.IsInSight(_controller.transform.position, _controller.Hideout.transform.position))
        {
            Debug.Log("In range of hideout position");
            _controller.Locomotion.MoveDirection(seek.GetDir(), _controller.Stats.Speed);

            if (Vector3.Distance(_controller.transform.position, _controller.Hideout.transform.position) < 0.3f)
            {
                _fsm.Transition(AgentManadaStates.FOLLOW_LEADER);
                return;
            }
        }
        else
        {
            _controller.Locomotion.MoveThroughPathNodes(currentPath, _controller.Stats.Speed, _controller.LineOfSight.IsInSight);
        }
    }

    public override void Sleep() { }
}