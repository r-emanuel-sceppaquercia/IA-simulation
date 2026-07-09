using UnityEngine;

public class AgentFleeState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;
    private AgentLocomotion _locomotion;

    public AgentFleeState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;
        _locomotion = controller.GetComponent<AgentLocomotion>();
    }

    public override void Execute()
    {
        _controller.useFlocking = false;

        if (_controller.myLeader != null && _controller.myLeader.Target != null)
        {
            // opposite vector to enemy
            Vector3 enemyPos = _controller.myLeader.Target.transform.position;
            Vector3 fleeDirection = (_controller.transform.position - enemyPos).normalized;
            fleeDirection.y = 0; // this avoids it going up or down, although i think we can do this in the inspector too, freezing the Y

            // 2. we give the vector to the locomotion
            _locomotion.SetMovementOverride(fleeDirection);
        }
        else
        {
            // If the leader doesn't have a target, we go back to following
            _fsm.Transition(AgentManadaStates.FOLLOW_LEADER);
        }
    }

    public override void Sleep()
    {
        // Once we're out of this state, we free the override
        _locomotion.SetMovementOverride(null);
    }
}