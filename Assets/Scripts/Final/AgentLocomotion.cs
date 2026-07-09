using UnityEngine;

[RequireComponent(typeof(AgentFSMController))]
[RequireComponent(typeof(FlockEntity))]
public class AgentLocomotion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Behaviour weight")]
    [SerializeField] private float seekWeight = 1f;
    [SerializeField] private float flockWeight = 1.5f;
    [SerializeField] private float avoidWeight = 5f;

    [Header("Evasion config")]
    [SerializeField] private float avoidanceRadius = 3f;
    [SerializeField] private LayerMask obstacleMask;

    private AgentFSMController _fsmController;
    private IFlockEntity _flockEntity;
    private Seek _seekBehavior;
    private ObstacleAvoidance _obstacleAvoidance;
    private Transform _currentLocomotionTarget;

    private Vector3? _overrideDirection = null;

    public void SetMovementOverride(Vector3? direction)
    {
        _overrideDirection = direction;
    }

    private void Awake()
    {
        _fsmController = GetComponent<AgentFSMController>();
        _flockEntity = GetComponent<FlockEntity>();
    }

    private void Update()
    {
       // if we have a direction to follow, we use it, otherwise we use Seek. 
       //Am I escaping?
        if (_overrideDirection == null)
        {
            //if I'm not, I need a target to follow / seek
            if (_fsmController.currentTarget == null) return;

            // if i have an objective, i do seek and avoidance
            UpdateSteeringBehaviors(_fsmController.currentTarget);
        }

        Vector3 finalDirection = CalculateMovementDirection();
        ApplyMovement(finalDirection);
    }

    private void UpdateSteeringBehaviors(Transform newTarget)
    {
        if (_currentLocomotionTarget != newTarget)
        {
            _currentLocomotionTarget = newTarget;
            _seekBehavior = new Seek(transform, _currentLocomotionTarget);
            _obstacleAvoidance = new ObstacleAvoidance(transform, _currentLocomotionTarget, avoidanceRadius, avoidWeight, obstacleMask);
        }
    }

    private Vector3 CalculateMovementDirection()
    {
        // If we're fleeing/escaping
        if (_overrideDirection.HasValue)
        {
            // combining the flee with avoidance 
            Vector3 fleeDir = _overrideDirection.Value;
            if (_obstacleAvoidance != null) fleeDir += _obstacleAvoidance.GetDir() * 0.5f;
            return fleeDir.normalized;
        }

        // IF we're following or pursuing
        Vector3 direction = Vector3.zero;
        if (_seekBehavior != null) direction += _seekBehavior.GetDir() * seekWeight;
        if (_flockEntity != null && _fsmController.useFlocking) direction += _flockEntity.Direction * flockWeight;
        if (_obstacleAvoidance != null) direction += _obstacleAvoidance.GetDir();

        return direction.normalized;
    }

    private void ApplyMovement(Vector3 direction)
    {
        if (direction == Vector3.zero) return;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}