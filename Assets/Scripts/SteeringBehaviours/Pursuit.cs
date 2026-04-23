using UnityEngine;

public class Pursuit : ISteering
{
    private Transform from;
    private Transform target;
    private float timePrediction;

    private VelocityTracker velocityTracker;

    public Pursuit(Transform from, Transform target, float timePrediction)
    {
        this.from = from;
        this.target = target;
        this.timePrediction = timePrediction;

        velocityTracker = target.GetComponent<VelocityTracker>();
    }

    public Vector3 GetDir()
    {
        Vector3 velocity = velocityTracker != null ? velocityTracker.Velocity : Vector3.zero;
        Vector3 predictedPos = target.position + velocity * timePrediction;

        return (predictedPos - from.position).normalized;
    }
}
