using DG.Tweening;
using UnityEngine;

public class PatrolState<T> : States<T>
{
    private HunterController hunterController;

    private FSM<T> hunterFSM;
    private T restInput;
    private T huntingInput;


    public PatrolState(HunterController hunterController, FSM<T> hunterFSM, T restInput, T huntingInput)
    {
        this.hunterController = hunterController;

        this.hunterFSM = hunterFSM;
        this.restInput = restInput;
        this.huntingInput = huntingInput;
    }

    public override void Awake()
    {
        Debug.Log("Patrol awake");

        DOVirtual.DelayedCall(0.3f, () =>
        {
            hunterController.Navmesh.ResumePatrolling();
            hunterController.View.ChangeMaterialColor(Color.green);
        });

        // Change anim to patrolling (if available)
    }

    public override void Execute()
    {
        Debug.Log("Patrol execute");

        hunterController.Navmesh.Move();

        hunterController.Model.ConsumeEnergy(5);

        // If the hunter is out of energy change to the rest state
        if (hunterController.Model.OutOfEnergy())
            hunterFSM.Transition(restInput);

        // If a boid is in range, change to hunting state
        if (hunterController.LineOfSight.EnemiesInRange())
        {
            hunterFSM.Transition(huntingInput);
        }
    }

    public override void Sleep()
    {
        Debug.Log("Patrol sleep");

        hunterController.Navmesh.StopPatrolling();
    }
}
