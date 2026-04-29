using System.Collections.Generic;
using UnityEngine;

public class CohesionBehavior : MonoBehaviour, IFlockBehaviour
{
    public float CohesionWeight;

    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        var center = Vector3.zero;
        int neighbours = 0; 

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] == entity) continue;

            center += entities[i].Position;
            neighbours++;
        }

        if (neighbours == 0) return Vector3.zero;

        center /= neighbours;

        return (center - entity.Position).normalized * CohesionWeight;
    }
}