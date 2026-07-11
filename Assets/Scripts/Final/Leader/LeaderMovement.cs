using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderMovement : MonoBehaviour
{

    public void MoveDirection(Vector3 direction, float speed)
    {
        direction.y = 0;

        transform.position += direction.normalized * speed * Time.deltaTime;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction.normalized, 10f * Time.deltaTime);
        }
    }

    public void MoveTo(Vector3 targetPosition, float speed)
    {
        Vector3 target = targetPosition;
        target.y = transform.position.y;

        Vector3 dir = target - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (dir.sqrMagnitude > 0.01f)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir.normalized, 10f * Time.deltaTime);
        }
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
