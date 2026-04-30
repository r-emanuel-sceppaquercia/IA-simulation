using UnityEngine;

public class HuntingState<T> : States<T>
{
    private HunterController hunterController;

    private Transform from;
    //private Transform target;

    private FSM<T> hunterFSM;
    private T restInput;
    private T patrolInput;

    private Transform currentTarget;

    public HuntingState(HunterController hunterController, FSM<T> hunterFSM, T restInput, T patrolInput)
    {
        this.hunterController = hunterController;

        this.from = hunterController.transform;

        this.hunterFSM = hunterFSM;
        this.restInput = restInput;
        this.patrolInput = patrolInput;
    }

    public override void Awake()
    {
        Debug.Log("Hunting awake");

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
            hunterController.Weapon.ResetShootStats();
            hunterController.Weapon.CalculateDistance(currentTarget);
        }
        else
        {
            // Lock target and aim
            hunterController.Weapon.AimAt(currentTarget);
            hunterController.Weapon.AimDelay();
        }

        // If we are out of couldown and have ammo
        if (hunterController.Weapon.CanShoot() && hunterController.Weapon.HasAmmo())
        {
            Debug.Log("Shot to the target");

            hunterController.Weapon.Shoot();
            hunterController.Model.ConsumeEnergy(50, false);
            hunterController.View.ShootAnimation();

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
        hunterController.Weapon.Reload();

        currentTarget = null;
        hunterController.Weapon.ResetShootStats();
    }

    private void CheckStateTransition()
    {
        // If the energy is empty, change to rest state
        if (hunterController.Model.OutOfEnergy())
            hunterFSM.Transition(restInput);

        // If there is no more enemies in range, change to patrol state
        if (!hunterController.LineOfSight.EnemiesInRange())
            hunterFSM.Transition(patrolInput);
    }

}
