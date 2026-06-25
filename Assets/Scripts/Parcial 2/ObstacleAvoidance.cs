using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    private Transform _from;
    private Transform _target;
    private float _radius;
    private float _avoidWeight;
    private LayerMask _mask;

    public ObstacleAvoidance(Transform from, Transform target, float radius, float avoidWeight, LayerMask mask)
    {
        _from = from;
        _target = target;
        _radius = radius;
        _avoidWeight = avoidWeight;
        _mask = mask;
    }

    public Vector3 GetDir()
    {
        Vector3 dir = (_target.position - _from.position).normalized;

        // what we hittin
        if (Physics.Raycast(_from.position, dir, out RaycastHit hit, _radius, _mask))
        {
            Vector3 avoidDir = hit.normal * _avoidWeight;

            return avoidDir;
        }

        // otherwise 0
        return Vector3.zero;
    }
}