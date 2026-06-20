using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    public List<PathfindingNode> CalculateDijkstra(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();
        var accumulativeCost = new Dictionary<PathfindingNode, int>();
        accumulativeCost.Add(startNode, 0);

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
                if (node.Block) continue;

                var newCost = node.Cost + accumulativeCost[current];

                if (!accumulativeCost.ContainsKey(node))
                {
                    accumulativeCost.Add(node, newCost);
                    pending.Enqueue(node, newCost);
                    parents.Add(node, current);
                }
                else if (accumulativeCost[node] > newCost)
                {
                    pending.Enqueue(node, newCost);
                    parents[node] = current;
                    accumulativeCost[node] = newCost;
                }
            }
        }

        return new List<PathfindingNode>();
    }

    public IEnumerator CalculateDijkstraCoroutine(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();
        var accumulativeCost = new Dictionary<PathfindingNode, int>();

        accumulativeCost.Add(startNode, 0);

        while (pending.Count > 0)
        {
            var current = pending.Dequeue();

            if (current == goalNode)
            {
                List<PathfindingNode> path = new();

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

            if (current != startNode && current != goalNode)
                current.ChangeColor(Color.yellow);

            foreach (var node in current.GetNeigbors)
            {
                if (node.Block) continue;

                var newCost = node.Cost + accumulativeCost[current];

                if (!accumulativeCost.ContainsKey(node))
                {
                    accumulativeCost.Add(node, newCost);
                    pending.Enqueue(node, newCost);
                    parents.Add(node, current);

                    if (current != startNode && current != goalNode)
                        node.ChangeColor(Color.blue);
                }
                else if (accumulativeCost[node] > newCost)
                {
                    pending.Enqueue(node, newCost);
                    parents[node] = current;
                    accumulativeCost[node] = newCost;

                    if (current != startNode && current != goalNode)
                        node.ChangeColor(Color.blue);
                }

                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
