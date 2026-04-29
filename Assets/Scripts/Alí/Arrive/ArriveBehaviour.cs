using UnityEngine;

public class Arrive : ISteering
{
    private Transform _from;
    private Transform _target;
    private float _maxSpeed;
    private float _slowingDistance;

    public Arrive(Transform from, Transform target, float maxSpeed, float slowingDistance)
    {
        _from = from;
        _target = target;
        _maxSpeed = maxSpeed;
        _slowingDistance = slowingDistance;
    }

    public Vector3 GetDir()
    {

        Vector3 desiredVelocity = _target.position - _from.position;
        float distance = desiredVelocity.magnitude;

        if (distance < _slowingDistance)
        {
            return desiredVelocity.normalized * (_maxSpeed * (distance / _slowingDistance));
        }
        else
        {
            return desiredVelocity.normalized * _maxSpeed;
        }
    }
}
