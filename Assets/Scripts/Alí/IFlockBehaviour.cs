using System.Collections.Generic;
using UnityEngine;

public interface IFlockBehaviour
{
    Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity);
}
