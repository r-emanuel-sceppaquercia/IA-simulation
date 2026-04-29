using UnityEngine;

public class Pursuit : ISteering
{
    private Transform from;
    private Transform target;
    private float timePrediction;

    public Pursuit(Transform from, Transform target, float timePrediction)
    {
        this.from = from;
        this.target = target;
        this.timePrediction = timePrediction;
    }

    public Vector3 GetDir()
    {
        Vector3 predictedPos = target.position + target.forward * timePrediction;
        return (predictedPos - from.position).normalized;
    }
}
