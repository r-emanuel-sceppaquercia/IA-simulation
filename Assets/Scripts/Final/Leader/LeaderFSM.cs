using UnityEngine;

public enum LeaderState
{
    MOVING,
    ROAMING,
    COMBAT,
    FLEE
}

public class LeaderFSM : MonoBehaviour
{
    private FSM<LeaderState> fsm;
    private LeaderController leaderController;

    private void Start()
    {
        leaderController = GetComponent<LeaderController>();

        SetUpFSM();
    }

    private void Update()
    {
        fsm.OnUpdate();
    }

    private void SetUpFSM()
    {
        fsm = new FSM<LeaderState>();

        var roamingState = new LeaderRoamingState<LeaderState>(fsm, leaderController);
        var movingState = new LeaderMovingState<LeaderState>(fsm, leaderController);
        var combatState = new LeaderCombatState<LeaderState>(fsm, leaderController);
        var fleeState = new LeaderFleeState<LeaderState>(fsm, leaderController);

        roamingState.AddTransition(LeaderState.MOVING, movingState);
        roamingState.AddTransition(LeaderState.COMBAT, combatState);

        movingState.AddTransition(LeaderState.ROAMING, roamingState);
        movingState.AddTransition(LeaderState.COMBAT, combatState);

        combatState.AddTransition(LeaderState.ROAMING, roamingState);
        combatState.AddTransition(LeaderState.FLEE, fleeState);

        fleeState.AddTransition(LeaderState.ROAMING, roamingState);

        fsm.SetInit(roamingState);
    }
}