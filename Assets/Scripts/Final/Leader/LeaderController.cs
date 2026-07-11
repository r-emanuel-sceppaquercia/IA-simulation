using UnityEngine;

public class LeaderController : MonoBehaviour
{
    public LeaderView View { get; private set; }
    public LeaderModel Model { get; private set; }
    public LeaderMovement Movement { get; private set; }
    public LeaderLineOfSight LineOfSight { get; private set; }

    [field: SerializeField] public Vector3 targetPoint { get; private set; }

    private void Awake()
    {
        View = GetComponent<LeaderView>();
        Model = GetComponent<LeaderModel>();
        Movement = GetComponent<LeaderMovement>();
        LineOfSight = GetComponent<LeaderLineOfSight>();

        targetPoint = Vector3.zero;
    }

    public void SetTargetPoint(Vector3 targetPoint)
    {
        this.targetPoint = targetPoint;
    }

    public bool HasMovingPoint()
    {
        return targetPoint != Vector3.zero;
    }
}
