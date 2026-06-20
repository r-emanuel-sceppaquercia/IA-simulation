using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{
    private Grid2D grid;
    private int xPos, yPos;

    [SerializeField] private List<PathfindingNode> neighbors = new();
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Material baseMaterial, blockMaterial, startMaterial, goalMaterial;

    public MeshRenderer MeshRenderer { get; private set; }
    public int Cost { get; private set; }
    public bool Block { get; private set; }

    public List<PathfindingNode> GetNeigbors
    {
        get
        {
            if (neighbors.Count > 0) return neighbors;

            var nodeUp = grid.GetNode(xPos, yPos + 1);
            if (nodeUp) neighbors.Add(nodeUp);

            var nodeDown = grid.GetNode(xPos, yPos - 1);
            if (nodeDown) neighbors.Add(nodeDown);

            var nodeRight = grid.GetNode(xPos + 1, yPos);
            if (nodeRight) neighbors.Add(nodeRight);

            var nodeLeft = grid.GetNode(xPos - 1, yPos);
            if (nodeLeft) neighbors.Add(nodeLeft);

            return neighbors;
        }
    }

    public void Initialize(Grid2D grid, int xPos, int yPos)
    {
        this.grid = grid;
        this.xPos = xPos;
        this.yPos = yPos;

        if (!MeshRenderer)
        {
            MeshRenderer = GetComponent<MeshRenderer>();
        }

        Cost = 1;
        costText.text = Cost.ToString();

        PathFindingManager.Instance.OnResetGrid += ResetNode;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PathFindingManager.Instance.startNode != null)
                PathFindingManager.Instance.startNode.MeshRenderer.material = baseMaterial;

            PathFindingManager.Instance.startNode = this;
            MeshRenderer.material = startMaterial;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (PathFindingManager.Instance.goalNode != null)
                PathFindingManager.Instance.goalNode.MeshRenderer.material = baseMaterial;

            PathFindingManager.Instance.goalNode = this;
            MeshRenderer.material = goalMaterial;
        }

        if (Input.GetMouseButtonDown(2))
        {
            Block = !Block;

            MeshRenderer.material = Block ? blockMaterial : baseMaterial;
        }


        if (Input.mouseScrollDelta.y > 0) SetNewCost(+1);
        if (Input.mouseScrollDelta.y < 0) SetNewCost(-1);
    }

    public void ChangeColor(Color color)
    {
        MeshRenderer.material.color = color;
    }

    public void SetNewCost(int value)
    {
        Cost = Mathf.Clamp(Cost + value, 1, int.MaxValue);
        costText.text = Cost.ToString();
    }

    public void ResetNode()
    {
        if (Block || PathFindingManager.Instance.startNode == this || PathFindingManager.Instance.goalNode == this)
            return;

        ChangeColor(baseMaterial.color);
    }

}
