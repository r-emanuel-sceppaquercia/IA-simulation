using System.Collections.Generic;
using UnityEngine;

public class HunterLOS : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask targetMask;

    [SerializeField] private List<Transform> visibleTargets;

    private LineOfSight lineOfSight = new LineOfSight();

    public float GetRange() => this.range;
    public float GetAngle() => this.angle;
    public LayerMask GetObstacleMask() => this.obstacleMask;
    public LayerMask GetTargetMask() => this.targetMask;

    void Start()
    {
        InvokeRepeating(nameof(UpdateFOV), 0f, 0.2f);
    }

    void UpdateFOV()
    {
        visibleTargets = lineOfSight.FindVisibleTargets(transform, range, angle, targetMask, obstacleMask);
    }

    public bool EnemiesInRange()
    {
        return visibleTargets.Count > 0;
    }

    public Transform GetClosestTarget()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var target in visibleTargets)
        {
            float dist = (target.position - transform.position).sqrMagnitude;

            if (dist < minDist)
            {
                minDist = dist;
                closest = target;
            }
        }

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, range);

        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * transform.forward;

        Gizmos.DrawLine(transform.position, transform.position + left * range);
        Gizmos.DrawLine(transform.position, transform.position + right * range);
    }

}
