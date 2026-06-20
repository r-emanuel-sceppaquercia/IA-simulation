
using UnityEngine;

public enum HeuristicType
{
    Euclidean,
    Manhattan
}

public class Heuristic
{

    public Heuristic()
    {

    }

    public int EuclideanHeuristic(Vector3 node, Vector3 goalNode)
    {
        return (int)Vector3.Distance(node, goalNode);
    }

    public int ManhattanHeuristic(Vector3 node, Vector3 goalNode)
    {
        return (int)(Mathf.Abs(goalNode.x - node.x) + Mathf.Abs(goalNode.y - node.y));
    }
}
