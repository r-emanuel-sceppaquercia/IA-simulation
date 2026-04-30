using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour : MonoBehaviour, IFlockBehaviour
{
    public float alignmentWeight;

    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        var dir = Vector3.zero;
        int neighbours = 0;

        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] == entity) continue;

            dir += entities[i].Direction;
            neighbours++; 
        }

        if (neighbours == 0) return Vector3.zero;

        dir /= neighbours;

        return dir.normalized * alignmentWeight;
    }
}
