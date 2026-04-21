using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour, IMove
{
    public HunterModel HunterModel { get; private set; }

    private QuestionNode questionEnergy;
    private QuestionNode questionSeeEnemy;
    private QuestionNode questionHasBullet;

    private ActionNode actionRest;
    private ActionNode actionPatrol;
    private ActionNode actionRecharge;
    private ActionNode actionShoot;

    private INode init;

    private Roulette roulette;
    private Dictionary<string, int> patrolDecisions = new Dictionary<string, int>();

    private void Awake()
    {
        this.HunterModel = new HunterModel(100, 100);
    }

    private void Start()
    {
        actionRest = new ActionNode(Rest);
        actionPatrol = new ActionNode(Patrol);
        actionRecharge = new ActionNode(Recharge);
        actionShoot = new ActionNode(Shoot);

        questionHasBullet = new QuestionNode(HunterModel.HasAmmo, actionShoot, actionRecharge);
        questionSeeEnemy = new QuestionNode(() => false, questionHasBullet, actionPatrol);
        questionEnergy = new QuestionNode(HunterModel.OutOfEnergy, questionSeeEnemy, actionRest);

        init = questionEnergy;

        roulette = new Roulette();
        patrolDecisions.Add("PreviousWaypoint", 50);
        patrolDecisions.Add("StartWaypoint", 50);
    }

    private void Update()
    {
        //init.Execute();
    }

    void IMove.Move(Vector3 dir)
    {
        throw new System.NotImplementedException();
    }

    public void Rest()
    {
        Debug.Log("Is resting");
    }

    public void Shoot()
    {
        Debug.Log("Is shooting");
    }

    public void Recharge()
    {
        Debug.Log("Is recharging");
    }

    public void Patrol()
    {
        Debug.Log("Is patrolling");
    }

    private void NewPatrolPoint()
    {
        roulette.Execute(patrolDecisions);
    }
}
