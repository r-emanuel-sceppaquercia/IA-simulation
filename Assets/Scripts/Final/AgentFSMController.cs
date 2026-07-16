using UnityEngine;

public enum AgentManadaStates
{
    FOLLOW_LEADER,
    COMBAT,
    FLEE
}

public class AgentFSMController : MonoBehaviour
{
    private FSM<AgentManadaStates> _fsm;

    public AgentLocomotion Locomotion { get; private set; }
    public AgentView View { get; private set; }
    public LeaderLineOfSight LineOfSight { get; private set; }
    public EntityStats Stats { get; private set; }

    public LeaderController MyLeader { get; private set; }

    public Transform currentTarget { get; set; }
    public Transform Hideout { get; private set; }
    public bool useFlocking { get; set; }

    private void Awake()
    {
        Locomotion = GetComponent<AgentLocomotion>();
        View = GetComponent<AgentView>();
        LineOfSight = GetComponent<LeaderLineOfSight>();
        Stats = GetComponent<EntityStats>();
        SetUpFSM();
    }

    private void SetUpFSM()
    {
        _fsm = new FSM<AgentManadaStates>();

        var followState = new AgentFollowState(this, _fsm);
        var combatState = new AgentCombatState(this, _fsm);
        var fleeState = new AgentFleeState(this, _fsm);

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