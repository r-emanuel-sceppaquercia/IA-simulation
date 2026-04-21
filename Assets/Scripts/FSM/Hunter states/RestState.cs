using UnityEngine;

public class RestState<T> : States<T>
{
    private HunterController hunterController;

    private FSM<T> hunterFSM;
    private T patrolInput;

    public RestState(HunterController hunterController, FSM<T> hunterFSM, T patrolInput)
    {
        this.hunterController = hunterController;
        this.hunterFSM = hunterFSM;
        this.patrolInput = patrolInput;
    }

    public override void Awake()
    {
        Debug.Log("Rest awake");

        hunterController.View.ChangeMaterialColor(Color.yellow);

        // Setup campfire and change anim to idle
        hunterController.View.RestAnimation(true);
    }

    public override void Execute()
    {
        Debug.Log("Rest execute");

        // Recover energy
        hunterController.Model.RecoverEnergy(10);

        // If energy is full change to patrol state
        if (hunterController.Model.FullEnergy())
            hunterFSM.Transition(patrolInput);
    }

    public override void Sleep()
    {
        Debug.Log("Rest sleep");

        // Pick up campfire and anim
        hunterController.View.RestAnimation(false);
    }
}
