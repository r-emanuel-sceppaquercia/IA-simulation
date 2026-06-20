using UnityEngine;

public enum AgentStates
{
    PATROL,
    ALERT,
    CHASE,
}

public class FSMAgent : MonoBehaviour
{
    private FSM<AgentStates> fsm;

    [SerializeField] private PathfindingAgent agent;

    private void Awake()
    {
        agent = GetComponent<PathfindingAgent>();

        SetUpFSM();
    }

    private void Update()
    {
        fsm.OnUpdate();
    }

    private void SetUpFSM()
    {
        fsm = new FSM<AgentStates>();

        var patrolState = new AgentPatrolState<AgentStates>(agent, fsm);
        var alertState = new AgentAlertState<AgentStates>(agent, fsm);
        var chaseState = new AgentChaseState<AgentStates>(agent, fsm);

        patrolState.AddTransition(AgentStates.CHASE, chaseState);
        patrolState.AddTransition(AgentStates.ALERT, alertState);

        alertState.AddTransition(AgentStates.PATROL, patrolState);
        alertState.AddTransition(AgentStates.CHASE, chaseState);

        chaseState.AddTransition(AgentStates.PATROL, patrolState);

        fsm.SetInit(patrolState);
    }
}
