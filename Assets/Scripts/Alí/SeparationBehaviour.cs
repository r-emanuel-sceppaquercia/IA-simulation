using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : MonoBehaviour, IFlockBehaviour
{
    public float separationWeight;
    public float range;

    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        var dir = Vector3.zero;
        for (int i = 0; i < entities.Count; i++)
        {
            var currentEntity = entities[i];
            if (currentEntity == entity) continue;

            if (Vector3.Distance(currentEntity.Position, entity.Position) < range)
            {
                var currentDir = (entity.Position - currentEntity.Position);

                dir += currentDir.normalized;
            }
        }

        return dir.normalized * separationWeight;
    }
}
