using UnityEngine;

public class HunterController : MonoBehaviour
{
    public HunterModel Model { get; private set; }
    public HunterView View { get; private set; }
    public HunterNavmesh Navmesh { get; private set; }
    public HunterLOS LineOfSight { get; private set; }
    public HunterAttackController AttackController { get; private set; }

    private void Awake()
    {
        Model = GetComponent<HunterModel>();
        Navmesh = GetComponent<HunterNavmesh>();
        View = GetComponent<HunterView>();
        LineOfSight = GetComponent<HunterLOS>();
        AttackController = GetComponent<HunterAttackController>();
    }
}
