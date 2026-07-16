using UnityEngine;

public class AgentFollowState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;

    public AgentFollowState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;
    }

    public override void Awake()
    {
        _controller.View.ChangeColor(Color.yellow);
    }

    public override void Execute()
    {
        // if low life, escape
        if (_controller.Stats.IsHealthLow() || (_controller.MyLeader != null && _controller.MyLeader.Model.OnLowHp()))
        {
            _fsm.Transition(AgentManadaStates.FLEE);
            return;
        }

        // if the leader or the agent detects an enemy, we attack
        if (_controller.MyLeader != null && _controller.MyLeader.LineOfSight.EnemiesInRange())
        {
            _fsm.Transition(AgentManadaStates.COMBAT);
            return;
        }

        // follow the leader
        if (_controller.MyLeader != null)
        {
            _controller.Locomotion.Move();
            _controller.currentTarget = _controller.MyLeader.transform;
            _controller.useFlocking = true;
        }
    }

    public override void Sleep()
    {
    }
}