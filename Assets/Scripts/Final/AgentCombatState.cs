using UnityEngine;

public class AgentCombatState : States<AgentManadaStates>
{
    private AgentFSMController _controller;
    private FSM<AgentManadaStates> _fsm;

    // added combat config -------
    private float _attackRange = 2.5f;    // Max distance to attack
    private float _attackCooldown = 1.5f; // Seconds between attacks
    private float _damage = 10f;          // Damage dealt per attack

    private float _lastAttackTime = 0f;   // Internal timer to handle cooldown

    private Transform enemy;

    public AgentCombatState(AgentFSMController controller, FSM<AgentManadaStates> fsm)
    {
        _controller = controller;
        _fsm = fsm;
    }

    public override void Awake()
    {
        _controller.View.ChangeColor(Color.red);

        // Get enemy from lineOfSight
        enemy = _controller.MyLeader.LineOfSight.GetClosestTarget();
    }

    public override void Execute()
    {
        // If the agent or the leader has low HP, flee to the hideout
        if (_controller.Stats.IsHealthLow() || (_controller.MyLeader != null && _controller.MyLeader.Model.OnLowHp()))
        {
            _fsm.Transition(AgentManadaStates.FLEE);
            return;
        }

        // If the leader lost sight of enemies or died, go back to following
        if (_controller.MyLeader == null || !_controller.MyLeader.LineOfSight.EnemiesInRange())
        {
            _fsm.Transition(AgentManadaStates.FOLLOW_LEADER);
            return;
        }

        if (enemy != null)
        {
            _controller.currentTarget = enemy;
            _controller.useFlocking = false;

            // Calculate actual distance to the enemy
            float distanceToEnemy = Vector3.Distance(_controller.transform.position, enemy.position);

            // If within attack range, stop and start attacking
            if (distanceToEnemy <= _attackRange)
            {
                // Check if enough time has passed since the last attack (Cooldown)
                if (Time.time >= _lastAttackTime + _attackCooldown)
                {
                    PerformAttack(enemy);
                    _lastAttackTime = Time.time; // Reset the attack timer
                }
            }
        }
    }

    private void PerformAttack(Transform enemy)
    {
        // Try to deal damage using the interface
        IDamageable targetStats = enemy.GetComponent<IDamageable>();

        if (targetStats != null)
        {
            targetStats.ReceiveDamage(_damage);
            Debug.Log($"{_controller.gameObject.name} attacked for {_damage} damage!");
        }
        else
        {
            // Fallback in case the other leader hasn't implemented IDamageable yet
            LeaderModel leaderTarget = enemy.GetComponent<LeaderModel>();
            if (leaderTarget != null)
            {
                Debug.LogWarning("Enemy leader hit, but it doesn't implement IDamageable yet!");
            }
        }

        // Trigger the attack animation
        Animator anim = _controller.GetComponentInChildren<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Attack"); //"Attack" Animation - Animatorrrrrrrrrrrrrrrrrrrrrrrr
        }
    }

    public override void Sleep() { }
}