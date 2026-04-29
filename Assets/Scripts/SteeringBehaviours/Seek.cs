using UnityEngine;

public class Seek : ISteering
{
    private Transform from;
    private Transform target;

    public Seek(Transform from, Transform target)
    {
        this.from = from;
        this.target = target;
    }

    public Vector3 GetDir()
    {
        return (target.position - from.position).normalized;
    }
}
