using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private List<PathNode> neighbours = new List<PathNode>();

    [field: SerializeField] public float Cost { get; private set; }

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        Cost = 1;
    }

    public void ChangeColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public List<PathNode> GetNeighbors() => neighbours;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        foreach (var node in neighbours)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
