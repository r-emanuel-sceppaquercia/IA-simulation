using UnityEngine;

public class AgentFleeState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;

    public AgentFleeState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;
    }

    public override void Execute()
    {
        _controller.useFlocking = true;

        if (_controller.myLeader != null)
        {
          //search for base
            Transform hideout = _controller.myLeader.Model.HerdType == Herd.A
                ? NodeManager.Instance.HideoutA
                : NodeManager.Instance.HideoutB;

            _controller.currentTarget = hideout;

            // if not in danger we go back to following
            if (!_controller.stats.IsHealthLow() && !_controller.myLeader.Model.OnLowHp())
            {
                _fsm.Transition(AgentManadaStates.FOLLOW_LEADER);
            }
        }
    }

    public override void Sleep() { }
}