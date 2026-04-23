using System.Collections.Generic;
using UnityEngine;

public class LineOfSight
{
    public bool IsInSight(Transform transform, Transform target, float range, float angle, LayerMask targetMask)
    {
        var dir = (target.position - transform.position).normalized;
        var distance = dir.magnitude;

        if (distance > range)
            return false;

        if (Vector3.Angle(transform.forward, dir) > angle / 2)
            return false;

        return Physics.Raycast(transform.position, dir, distance, targetMask);
    }

    public List<Transform> FindVisibleTargets(Transform transform, float range, float angle, LayerMask targetMask, LayerMask obstacleMask)
    {
        List<Transform> visibleTargets = new List<Transform>();

        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, range, targetMask);

        Debug.Log("Targets in radius: " + targetsInRadius.Length);

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
