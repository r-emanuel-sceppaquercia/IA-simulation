using System;
using System.Collections.Generic;
using UnityEngine;

public class FlockEntity : MonoBehaviour, ISteering, IFlockEntity
{
    [SerializeField] private LayerMask _maskEntity;
    private List<IFlockBehaviour> _behaviours = new();
    [SerializeField] private float _radius;
    private Vector3 _dir;

    [SerializeField] private LayerMask _maskHunter; 
    [SerializeField] private float _hunterVisionRadius; 
    private Transform _targetHunter; 
    public Vector3 Direction => _dir;
    public Vector3 Position => transform.position;

    [SerializeField] private LayerMask _maskFood;
    [SerializeField] private float _foodVisionRadius = 10f;
    private Transform _targetFood;

    private Wander _wanderBehavior;

    private Boid _boidStats;

    private INode _rootNode;

    private void Awake()
    {
        var behaviors = GetComponents<IFlockBehaviour>();
        _behaviours = new List<IFlockBehaviour>(behaviors);
    }

    private void Start()
    {
        _boidStats = GetComponent<Boid>();

        INode actionArrive = new ActionNode(ApplyArrive);
        INode actionEvade = new ActionNode(ApplyEvade);
        INode actionFlocking = new ActionNode(ApplyFlocking);
        INode actionWander = new ActionNode(ApplyWander);

        _wanderBehavior = new Wander(_boidStats.GetSpeed());

        // Boids near?
        INode questionBoids = new QuestionNode(AreBoidsNear, actionFlocking, actionWander);

        // Hunter near?
        INode questionHunter = new QuestionNode(IsHunterNear, actionEvade, questionBoids);

        // Burger near?
        _rootNode = new QuestionNode(IsFoodNear, actionArrive, questionHunter);
    }

    private void Update()
    {
        if (!_boidStats.IsAlive) return;

        if (_rootNode != null)
        {
            _rootNode.Execute();
        }
    }

        private bool IsFoodNear()
    {
        
        Collider[] foods = Physics.OverlapSphere(transform.position, _foodVisionRadius, _maskFood);
        float closestDistance = Mathf.Infinity;
        _targetFood = null;

        for (int i = 0; i < foods.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, foods[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                _targetFood = foods[i].transform;
            }
        }

        return _targetFood != null;
    }

    private bool IsHunterNear()
    {
        Collider[] hunters = Physics.OverlapSphere(transform.position, _hunterVisionRadius, _maskHunter);

        if (hunters.Length > 0)
        {
            _targetHunter = hunters[0].transform; 
            return true;
        }

        _targetHunter = null;
        return false;
    }

    private bool AreBoidsNear()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, _radius, _maskEntity);
        return objs.Length > 1;
    }

    private void ApplyFlocking()
    {
        Vector3 movementDirection = GetDir();

        movementDirection.y = 0f;

        if (movementDirection != Vector3.zero)
        {
            _boidStats.Move(movementDirection.normalized);

            transform.forward = movementDirection.normalized;
        }
    }

    private void ApplyArrive()
    {
        if (_targetFood == null) return;

        float distance = Vector3.Distance(transform.position, _targetFood.position);

        if (distance < 0.5f)
        {
            Destroy(_targetFood.gameObject);
            _targetFood = null;
            return;
        }

        Arrive arriveBehaviour = new Arrive(transform, _targetFood, _boidStats.GetSpeed(), 3f);
        Vector3 moveDir = arriveBehaviour.GetDir();

        _boidStats.Move(moveDir.normalized);

        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir.normalized;
        }
    }
    private void ApplyEvade()
    {
        if (_targetHunter == null) return;

        Evade evadeBehavior = new Evade(transform, _targetHunter, _boidStats.GetSpeed());

        Vector3 moveDir = evadeBehavior.GetDir();

        transform.position += moveDir * Time.deltaTime;

        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir.normalized;
        }
    }

    private void ApplyWander()
    {
        Vector3 moveDir = _wanderBehavior.GetDir();

        _boidStats.Move(moveDir.normalized);

        if (moveDir != Vector3.zero)
        {
            transform.forward = moveDir.normalized;
        }
    }

    public Vector3 GetDir()
    {
        var objs = Physics.OverlapSphere(transform.position, _radius, _maskEntity);
        var entities = new List<IFlockEntity>();

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].transform.position == transform.position) continue;
            var currEntity = objs[i].GetComponent<IFlockEntity>();
            if (currEntity != null)
                entities.Add(currEntity);
        }

        _dir = Vector3.zero;
        for (int i = 0; i < _behaviours.Count; i++)
        {
            _dir += _behaviours[i].GetDir(entities, this);
        }

        return _dir.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius); // Flocking

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _foodVisionRadius); // Burger

        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, _hunterVisionRadius); // Hunter
    }

}

