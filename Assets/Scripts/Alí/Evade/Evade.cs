using UnityEngine;

public class Evade : ISteering
{
    private Transform _from;
    private Transform _target; // el cazador
    private float _maxSpeed;

    public Evade(Transform from, Transform target, float maxSpeed)
    {
        _from = from;
        _target = target;
        _maxSpeed = maxSpeed;
    }

    public Vector3 GetDir()
    {
        // al invertirlo va al lado contrario
        Vector3 desiredVelocity = _from.position - _target.position;

        return desiredVelocity.normalized * _maxSpeed;
    }
}