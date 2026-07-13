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

    public override void Execute()
    {
        // if low life, escape
        if (_controller.stats.IsHealthLow() || (_controller.myLeader != null && _controller.myLeader.Model.OnLowHp()))
        {
            _fsm.Transition(AgentManadaStates.FLEE);
            return;
        }

        // if leader detects an enemy, we attack
        if (_controller.myLeader != null && _controller.myLeader.LineOfSight.EnemiesInRange())
        {
            _fsm.Transition(AgentManadaStates.COMBAT);
            return;
        }

        // follow the leader
        if (_controller.myLeader != null)
        {
            _controller.currentTarget = _controller.myLeader.transform;
            _controller.useFlocking = true;
        }
    }

    public override void Sleep() { }
}