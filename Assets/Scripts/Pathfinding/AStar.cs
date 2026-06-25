using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<T>
{
    public List<T> CalculateAStar(T startNode,
        Func<T, bool> satisfies,
        Func<T, List<T>> getNeighbors,
        Func<T, float> getCost,
        Func<T, float> heuristic,
        int watchdog = 200)
    {
        var pending = new PriorityQueue<T>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<T, T>();

        var accumulativeCost = new Dictionary<T, float>();
        accumulativeCost.Add(startNode, 0);

        HashSet<T> visited = new HashSet<T>();

        while (pending.Count > 0)
        {
            watchdog--;
            if (watchdog <= 0)
                return new List<T>();

            var current = pending.Dequeue();
            visited.Add(current);

            if (satisfies(current))
            {
                List<T> path = new();
                path.Add(current);

                while (parents.ContainsKey(path[path.Count - 1]))
                {
                    var lastNode = path[path.Count - 1];
                    path.Add(parents[lastNode]);
                }

                path.Reverse();
                return path;
            }

            foreach (var node in getNeighbors(current))
            {
                if (visited.Contains(node))
                    continue;

                var nodeCost = getCost(node);
                var totalCost = accumulativeCost[current] + nodeCost;

                if (accumulativeCost.ContainsKey(node) && accumulativeCost[node] < totalCost)
                    continue;

                accumulativeCost[node] = totalCost;
                parents[node] = current;

                pending.Enqueue(node, totalCost + heuristic(node));
            }
        }

        return new List<T>();
    }

    public List<PathfindingNode> CalculateAStar(PathfindingNode startNode, PathfindingNode goalNode, HeuristicType heuristicType)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();

        var accumulativeCost = new Dictionary<PathfindingNode, float>();
        accumulativeCost.Add(startNode, 0);

        var heuristic = new Heuristic();

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
                }

                path.Add(startNode);
                path.Reverse();
                return path;
            }

            foreach (var node in current.GetNeigbors)
            {
                if (node.Block) continue;

                var newCost = accumulativeCost[current] + node.Cost;
                var priority = newCost + (heuristicType.Equals(HeuristicType.Euclidean) ?
                    heuristic.EuclideanHeuristic(node.transform.position, goalNode.transform.position) :
                    heuristic.ManhattanHeuristic(node.transform.position, goalNode.transform.position));

                if (!accumulativeCost.ContainsKey(node))
                {
                    accumulativeCost.Add(node, newCost);
                    pending.Enqueue(node, priority);
                    parents.Add(node, current);
                }
                else if (accumulativeCost[node] > newCost)
                {
                    accumulativeCost[node] = newCost;
                    pending.Enqueue(node, priority);
                    parents[node] = current;
                }
            }
        }

        return new List<PathfindingNode>();
    }

    public IEnumerator CalculateAStarCoroutine(PathfindingNode startNode, PathfindingNode goalNode, HeuristicType heuristicType)
    {
        var pending = new PriorityQueue<PathfindingNode>();
        pending.Enqueue(startNode, 0);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();

        var accumulativeCost = new Dictionary<PathfindingNode, float>();
        accumulativeCost.Add(startNode, 0);

        var heuristic = new Heuristic();

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

                path.Add(startNode);
                path.Reverse();
                break;
            }

            if (current != startNode && current != goalNode)
                current.ChangeColor(Color.yellow);

            foreach (var node in current.GetNeigbors)
            {
                if (node.Block) continue;

                var newCost = accumulativeCost[current] + node.Cost;
                var priority = newCost + (heuristicType.Equals(HeuristicType.Euclidean) ?
                    heuristic.EuclideanHeuristic(node.transform.position, goalNode.transform.position) :
                    heuristic.ManhattanHeuristic(node.transform.position, goalNode.transform.position));

                if (!accumulativeCost.ContainsKey(node))
                {
                    accumulativeCost.Add(node, newCost);
                    pending.Enqueue(node, priority);
                    parents.Add(node, current);

                    if (current != startNode && current != goalNode)
                        current.ChangeColor(Color.blue);
                }
                else if (accumulativeCost[node] > newCost)
                {
                    accumulativeCost[node] = newCost;
                    pending.Enqueue(node, priority);
                    parents[node] = current;

                    if (current != startNode && current != goalNode)
                        current.ChangeColor(Color.blue);
                }

                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
