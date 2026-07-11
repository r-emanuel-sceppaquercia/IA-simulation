using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    private Transform _from;
    private Vector3 _targetPos;
    private float _radius;
    private float _avoidWeight;
    private LayerMask _mask;

    public ObstacleAvoidance(Transform from, Transform target, float radius, float avoidWeight, LayerMask mask)
    {
        _from = from;
        _targetPos = target.position;
        _radius = radius;
        _avoidWeight = avoidWeight;
        _mask = mask;
    }

    public ObstacleAvoidance(Transform from, Vector3 targetPos, float radius, float avoidWeight, LayerMask mask)
    {
        _from = from;
        _targetPos = targetPos;
        _radius = radius;
        _avoidWeight = avoidWeight;
        _mask = mask;
    }

    public void SetTarget(Vector3 target)
    {
        _targetPos = target;
    }

    public Vector3 GetDir()
    {
        Vector3 dir = (_targetPos - _from.position).normalized;

        Vector3 origin = _from.position + Vector3.up * 0.5f;

        // Línea azul: hacia dónde quiere ir el agente
        Debug.DrawLine(origin, _targetPos, Color.blue);

        // Rayo rojo: lo que está chequeando el avoidance
        Debug.DrawRay(_from.position, dir * _radius, Color.red);

        if (Physics.Raycast(_from.position, dir, out RaycastHit hit, _radius, _mask))
        {
            Debug.Log("Golpeando: " + hit.collider.name);

            // Rayo amarillo: normal del obstáculo impactado
            Debug.DrawRay(hit.point, hit.normal * 2f, Color.yellow);

            return hit.normal * _avoidWeight;
        }

        return Vector3.zero;
    }
}