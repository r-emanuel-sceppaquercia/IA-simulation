using System;
using System.Collections.Generic;

public class ThetaStar
{
    public List<PathfindingNode> CalculateThetaStar(
        PathfindingNode start,
        PathfindingNode goal,
        HeuristicType heuristicType,
        Func<PathfindingNode, PathfindingNode, bool> inSight)
    {
        PriorityQueue<PathfindingNode> pending = new PriorityQueue<PathfindingNode>();

        Dictionary<PathfindingNode, PathfindingNode> parents = new();
        Dictionary<PathfindingNode, float> accumulativeCost = new();


        pending.Enqueue(start, 0);
        accumulativeCost.Add(start, 0f);
        var heuristic = new Heuristic();

        while (pending.Count > 0)
        {
            var current = pending.Dequeue();

            if (current == goal)
            {
                var path = new List<PathfindingNode>();

                while (current != start)
                {
                    path.Add(current);
                    current = parents[current];
                }

                path.Add(start);
                path.Reverse();
                return path;
            }

            foreach (var node in current.GetNeigbors)
            {
                if (node.Block)
                    continue;

                PathfindingNode currentParent = current;
                if (parents.ContainsKey(current) && inSight(parents[current], node))
                    currentParent = parents[current];


                var newCost = currentParent.Cost + accumulativeCost[current];
                var priority = 0f;

                switch (heuristicType)
                {
                    case HeuristicType.Euclidean:
                        priority = newCost + heuristic.EuclideanHeuristic(node.transform.position, goal.transform.position);
                        break;
                    case HeuristicType.Manhattan:
                        priority = newCost + heuristic.ManhattanHeuristic(node.transform.position, goal.transform.position);
                        break;
                    default: break;
                }

                if (!accumulativeCost.ContainsKey(node))
                {
                    accumulativeCost.Add(node, newCost);
                    pending.Enqueue(node, priority);
                    parents.Add(node, currentParent);
                }
                else if (accumulativeCost[node] > newCost)
                {
                    pending.Enqueue(node, priority);
                    parents[node] = currentParent;
                    accumulativeCost[node] = newCost;
                }
            }

        }

        return new List<PathfindingNode>();
    }

    public List<PathfindingNode> CalculateThetaStarBasedOnAStar(PathfindingNode start, PathfindingNode goal, HeuristicType heuristicType, Func<PathfindingNode, PathfindingNode, bool> inSight)
    {
        var aStar = new AStar<PathfindingNode>();
        var nodeList = aStar.CalculateAStar(start, goal, heuristicType);

        int currentNodeIndex = 0;

        while (currentNodeIndex + 2 < nodeList.Count)
        {
            if (inSight(nodeList[currentNodeIndex], nodeList[currentNodeIndex + 2]))
                nodeList.RemoveAt(currentNodeIndex + 1);
            else
                currentNodeIndex++;
        }
        return nodeList;
    }
}
