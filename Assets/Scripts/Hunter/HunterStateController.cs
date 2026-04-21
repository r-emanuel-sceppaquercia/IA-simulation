using UnityEngine;

public class HunterStateController : MonoBehaviour
{
    private FSM<FSMState> hunterFsm;

    private HunterController hunterController;

    [SerializeField] private Transform target;

    private void Awake()
    {
        hunterController = GetComponent<HunterController>();
    }

    private void Start()
    {
        SetUpFSM();
    }

    private void Update()
    {
        hunterFsm.OnUpdate();
    }

    private void SetUpFSM()
    {
        hunterFsm = new FSM<FSMState>();

        var rest = new RestState<FSMState>(hunterController, hunterFsm, FSMState.PATROL);
        var patrol = new PatrolState<FSMState>(hunterController, hunterFsm, FSMState.REST, FSMState.HUNTING);
        var hunting = new HuntingState<FSMState>(hunterController, hunterFsm, FSMState.REST, FSMState.PATROL);

        rest.AddTransition(FSMState.PATROL, patrol);
        patrol.AddTransition(FSMState.REST, rest);
        patrol.AddTransition(FSMState.HUNTING, hunting);
        hunting.AddTransition(FSMState.PATROL, patrol);
        hunting.AddTransition(FSMState.REST, rest);

        hunterFsm.SetInit(rest);
    }
}
