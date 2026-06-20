using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS
{
    public List<PathfindingNode> CalculateBFS(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new Queue<PathfindingNode>();
        pending.Enqueue(startNode);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();

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
                    pending.Enqueue(node);
                    parents.Add(node, current);
                }
            }
        }

        return new List<PathfindingNode>();
    }

    public IEnumerator CalculateBFSCoroutine(PathfindingNode startNode, PathfindingNode goalNode)
    {
        var pending = new Queue<PathfindingNode>();
        pending.Enqueue(startNode);

        var parents = new Dictionary<PathfindingNode, PathfindingNode>();

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
                    pending.Enqueue(node);
                    parents.Add(node, current);

                    if (current != startNode && current != goalNode)
                        node.ChangeColor(Color.blue);
                }

                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
