using System.Collections.Generic;
using UnityEngine;

public enum Herd
{
    A,
    B
}

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance;

    [field: SerializeField] private List<PathNode> nodes;

    [field: SerializeField] public Transform HideoutA { get; private set; }
    [field: SerializeField] public Transform HideoutB { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetHideoutPosition(Herd hideout)
    {
        return hideout.Equals(Herd.A) ? HideoutA.transform.position : HideoutB.transform.position;
    }

    public PathNode GetClosestNodeToHideout(Herd hideout)
    {
        var hideoutPosition = hideout.Equals(Herd.A) ? HideoutA : HideoutB;
        return GetClosestNodeToPosition(hideoutPosition.transform.position);
    }

    public PathNode GetClosestNodeToPosition(Vector3 position, float distance = 250)
    {
        PathNode closest = null;

        foreach (var node in nodes)
        {
            float sqrDistance = (node.transform.position - position).sqrMagnitude;

            if (sqrDistance < distance)
            {
                distance = sqrDistance;
                closest = node;
            }
        }

        return closest;
    }

    public PathNode GetRandomNode()
    {
        return nodes[Random.Range(0, nodes.Count)];
    }
}
