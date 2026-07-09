using UnityEngine;

public class AgentCombatState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;

    public AgentCombatState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;
    }

    public override void Execute()
    {
        // Transition, in case of low life I have to escape/flee
        if (_controller.stats.IsHealthLow())
        {
            _fsm.Transition(AgentManadaStates.FLEE);
            return;
        }

        // Transition in case Leader lost track of the other leader / target, so we go back to following him
        if (_controller.myLeader == null || !_controller.myLeader.EnemyInSight || _controller.myLeader.Target == null)
        {
            _fsm.Transition(AgentManadaStates.FOLLOW_LEADER);
            return;
        }

        // go straight to the enemy / target
        _controller.currentTarget = _controller.myLeader.Target.transform;
        _controller.useFlocking = false;

        // TO DO: damage logic
    }

    public override void Sleep() { }
}
