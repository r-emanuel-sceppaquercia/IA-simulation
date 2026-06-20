using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSGreedy
{
    public List<PathfindingNode> CalculateBFSGreedy(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();
        var heuristic = new Heuristic();

        while (pending.Count > 0)
        {
            var current = pending.Dequeue();

            if (current == goalNode)
            {
                var path = new List<PathfindingNode>();

                while (current != startNode)
                {
                    path.Add(current);
                    current = parents[current];
                }

                path.Add(current);
                path.Reverse();
                return path;
            }

            foreach (var node in current.GetNeigbors)
            {
                if (!node.Block && !parents.ContainsKey(node))
                {
                    var newCost = heuristic.EuclideanHeuristic(node.transform.position, goalNode.transform.position);
                    pending.Enqueue(node, newCost);
                    parents.Add(node, current);
                }
            }
        }

        return new List<PathfindingNode>();
    }

    public IEnumerator CalculateBFSGreedyCoroutine(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();
        var heuristic = new Heuristic();

        while (pending.Count > 0)
        {
            var current = pending.Dequeue();

            if (current == goalNode)
            {
                var path = new List<PathfindingNode>();

                while (current != startNode)
                {
                    path.Add(current);
                    current = parents[current];

                    if (current != startNode && current != goalNode)
                        current.ChangeColor(Color.green);

                    yield return new WaitForSeconds(0.025f);
                }

                path.Add(current);
                path.Reverse();
                break;
            }

            foreach (var node in current.GetNeigbors)
            {
                if (current != startNode && current != goalNode)
                    current.ChangeColor(Color.yellow);

                if (!node.Block && !parents.ContainsKey(node))
                {
                    var newCost = heuristic.EuclideanHeuristic(node.transform.position, goalNode.transform.position);
                    pending.Enqueue(node, newCost);
                    parents.Add(node, current);

                    if (current != startNode && current != goalNode)
                        node.ChangeColor(Color.blue);
                }
            }

            yield return new WaitForSeconds(0.025f);
        }
    }
}
