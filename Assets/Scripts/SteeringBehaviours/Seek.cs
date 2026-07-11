using UnityEngine;

public class Seek : ISteering
{
    private Transform from;

    private Transform targetTransform;
    private Vector3 targetPos;

    private bool useTransform;

    public Seek(Transform from, Transform target)
    {
        this.from = from;
        targetTransform = target;
        useTransform = true;
    }

    public Seek(Transform from, Vector3 targetPos)
    {
        this.from = from;
        this.targetPos = targetPos;
        useTransform = false;
    }

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
        useTransform = false;
    }

    public Vector3 GetDir()
    {
        Vector3 target = useTransform ? targetTransform.position : targetPos;
        return (target - from.position).normalized;
    }
}
