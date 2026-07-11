using UnityEngine;

public class LeaderCombatState<T> : States<T>
{
    private LeaderController leaderController;
    private FSM<LeaderState> fsm;

    private Transform enemy;

    private ISteering seek;
    private ISteering avoidance;

    public LeaderCombatState(FSM<LeaderState> fsm, LeaderController leaderController)
    {
        this.fsm = fsm;
        this.leaderController = leaderController;
    }

    public override void Awake()
    {
        leaderController.View.ChangeColor(Color.red);

        enemy = leaderController.LineOfSight.GetClosestTarget();

        seek = new Seek(leaderController.transform, enemy);

        avoidance = new ObstacleAvoidance(
            leaderController.transform,
            enemy,
            leaderController.Model.AvoidRadius,
            leaderController.Model.AvoidWeight,
            leaderController.LineOfSight.GetObstacleMask());
    }

    public override void Execute()
    {
        if (leaderController.Model.OnLowHp())
        {
            fsm.Transition(LeaderState.FLEE);
            return;
        }

        // Enemy is null (dead) or in flee state, transition to roaming
        if (enemy == null || !leaderController.LineOfSight.EnemiesInRange())
        {
            fsm.Transition(LeaderState.ROAMING);
            return;
        }

        // If not yet in attack range -> chase
        if (Vector3.Distance(leaderController.transform.position, enemy.position) > leaderController.Model.AttackRange)
        {
            Vector3 dir = (seek.GetDir() + avoidance.GetDir()).normalized;
            leaderController.Movement.MoveDirection(dir, leaderController.Model.Speed);
        }
        else
        {
            // TODO: perform attack (flee just for test)
            fsm.Transition(LeaderState.FLEE);
            return;
        }
    }

    public override void Sleep()
    {
        enemy = null;
    }
}
