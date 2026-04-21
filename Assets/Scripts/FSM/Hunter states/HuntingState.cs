using UnityEngine;

public class HuntingState<T> : States<T>
{
    private HunterController hunterController;

    private Transform from;
    //private Transform target;

    private FSM<T> hunterFSM;
    private T restInput;
    private T patrolInput;

    private ISteering steering;

    private Transform currentTarget;

    public HuntingState(HunterController hunterController, FSM<T> hunterFSM, T restInput, T patrolInput)
    {
        this.hunterController = hunterController;

        this.from = hunterController.transform;
        //this.target = target;

        this.hunterFSM = hunterFSM;
        this.restInput = restInput;
        this.patrolInput = patrolInput;
    }

    public override void Awake()
    {
        Debug.Log("Hunting awake");

        //steering = new Seek(from, target);

        hunterController.View.ChangeMaterialColor(Color.red);

        // Change to hunting anim
        hunterController.View.HuntingAnimation(true);
    }

    public override void Execute()
    {
        Debug.Log("Hunting execute");

        // If there is no target we select one
        if (currentTarget == null)
        {
            currentTarget = hunterController.LineOfSight.GetClosestTarget();
            hunterController.AttackController.ResetShootStats();
        }
        else
        {
            // Lock target and aim
            hunterController.AttackController.AimAt(currentTarget);
            hunterController.AttackController.AimDelay();
        }


        // Wait a couple of seconds to aim

        // If we are out of couldown and have ammo
        if (hunterController.AttackController.CanShot() && hunterController.Model.HasAmmo())
        {
            Debug.Log("Shot to the target");

            //hunterController.AttackController.Shoot();
            hunterController.Model.UseAmmo();
            hunterController.Model.ConsumeEnergy(20);

            currentTarget = null;
        }

        // After the shot we check transition conditions or then repeat
        CheckStateTransition();
    }

    public override void Sleep()
    {
        Debug.Log("Hunting sleep");

        // Turn off animation
        hunterController.View.HuntingAnimation(false);

        // Reload gun
        hunterController.Model.Reload();
    }

    private void CheckStateTransition()
    {
        // If the energy is empty, change to rest state
        if (hunterController.Model.OutOfEnergy())
            hunterFSM.Transition(restInput);

        // If there is no more enemies in range, change to patrol state
        if (!hunterController.LineOfSight.EnemiesInRange())
        {
            hunterFSM.Transition(patrolInput);
        }
    }

}
