using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private List<PathNode> neighbours = new List<PathNode>();

    private void Start()
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = Color.red;
    }

    public List<PathNode> GetNeighbors()
    {
        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        foreach (var node in neighbours)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
