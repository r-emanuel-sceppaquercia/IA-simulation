using UnityEngine;

public enum AgentManadaStates
{
    FOLLOW_LEADER,
    COMBAT,
    FLEE
}

[RequireComponent(typeof(EntityStats))] 
public class AgentFSMController : MonoBehaviour
{
    private FSM<AgentManadaStates> _fsm;
    
    public EntityStats stats { get; private set; }
    public PathfindingAgent myLeader; 

    public Transform currentTarget { get; set; }
    public bool useFlocking { get; set; }

    private void Awake()
    {
        stats = GetComponent<EntityStats>();
        SetUpFSM();
    }

    private void SetUpFSM()
    {
        _fsm = new FSM<AgentManadaStates>();

        var followState = new AgentFollowState(this, _fsm);
        var combatState = new AgentCombatState(this, _fsm);
        var fleeState = new AgentFleeState(this, _fsm);

        // Defining the transitions
        followState.AddTransition(AgentManadaStates.COMBAT, combatState);
        followState.AddTransition(AgentManadaStates.FLEE, fleeState);

        combatState.AddTransition(AgentManadaStates.FOLLOW_LEADER, followState);
        combatState.AddTransition(AgentManadaStates.FLEE, fleeState);

        fleeState.AddTransition(AgentManadaStates.FOLLOW_LEADER, followState);

        _fsm.SetInit(followState);
    }

    private void Update()
    {
        _fsm.OnUpdate();
    }
}