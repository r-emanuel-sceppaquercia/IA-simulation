using System;
using System.Collections.Generic;
using UnityEngine;

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
    //private IFlockEntity _flockEntity;
    private Seek _seekBehavior;
    private ObstacleAvoidance _obstacleAvoidance;
    private Transform _currentLocomotionTarget;

    private void Awake()
    {
        _fsmController = GetComponent<AgentFSMController>();
        //_flockEntity = GetComponent<FlockEntity>();
    }

    private void Update()
    {

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

    public void Move()
    {
        //if I don't have a target to follow / seek
        if (_fsmController.currentTarget == null) return;

        // if i have an objective, i do seek and avoidance
        UpdateSteeringBehaviors(_fsmController.currentTarget);

        Vector3 finalDirection = CalculateMovementDirection();
        ApplyMovement(finalDirection);
    }

    public void MoveDirection(Vector3 direction, float speed)
    {
        direction.y = 0;

        transform.position += direction.normalized * speed * Time.deltaTime;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, 10f * Time.deltaTime);
        }
    }

    private Vector3 CalculateMovementDirection()
    {
        // IF we're following, pursuing or fleeing to base
        Vector3 direction = Vector3.zero;

        if (_seekBehavior != null) direction += _seekBehavior.GetDir() * seekWeight;

        //if (_flockEntity != null && _fsmController.useFlocking) direction += _flockEntity.Direction * flockWeight;

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

    public void MoveThroughPathNodes(List<PathNode> path, float speed, Func<Vector3, Vector3, bool> isInSight)
    {
        if (path.Count == 0) return;

        while (path.Count > 1 && isInSight(transform.position, path[1].transform.position))
        {
            path.RemoveAt(0);
        }

        var dir = new Vector3(path[0].transform.position.x, path[0].transform.position.y, path[0].transform.position.z) - transform.position;

        transform.position += dir.normalized * speed * Time.deltaTime;

        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, 10f * Time.deltaTime);

        if (dir.magnitude < 0.1f)
        {
            path.RemoveAt(0);
        }
    }
}