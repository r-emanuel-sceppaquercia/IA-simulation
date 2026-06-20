using System.Collections.Generic;
using UnityEngine;

public class LineOfSight
{
    public bool IsInSight(Transform transform, Vector3 targetPosition, float range, float angle, LayerMask obstacleMask)
    {
        Vector3 dir = targetPosition - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();

        Debug.Log(distance);
        Debug.DrawRay(transform.position, dir * distance, Color.red);

        if (distance > range)
            return false;

        if (Vector3.Angle(transform.forward, dir) > angle / 2)
            return false;

        return !Physics.Raycast(transform.position, dir, distance, obstacleMask);
    }

    public bool IsInLineOfSightToPoint(Transform transform, Vector3 point, LayerMask obstacleMask)
    {
        Vector3 dir = point - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();

        return !Physics.Raycast(transform.position, dir, distance, obstacleMask);
    }

    public List<Transform> FindVisibleTargets(Transform transform, float range, float angle, LayerMask targetMask, LayerMask obstacleMask)
    {
        List<Transform> visibleTargets = new List<Transform>();

        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, range, targetMask);

        foreach (Collider target in targetsInRadius)
        {
            Transform targetTransform = target.transform;


            var damageable = targetTransform.GetComponent<IDamageable>();
            if (damageable == null || !damageable.IsAlive) continue;


            Vector3 dir = (targetTransform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dir) < angle / 2)
            {
                float distance = Vector3.Distance(transform.position, targetTransform.position);

                if (!Physics.Raycast(transform.position, dir, distance, obstacleMask))
                {
                    visibleTargets.Add(targetTransform);
                }
            }
        }

        return visibleTargets;
    }

}
