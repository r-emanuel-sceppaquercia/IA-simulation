using UnityEngine;

public class Grid2D : MonoBehaviour
{

    [SerializeField] private PathfindingNode nodePrefab;

    [SerializeField] private int width = 10, height = 10;

    [SerializeField, Range(1f, 2f)] private float offset;

    private PathfindingNode[,] grid;

    public void Start()
    {
        grid = new PathfindingNode[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                PathfindingNode newNode = Instantiate(nodePrefab, new Vector3(i, j, 0) * offset, Quaternion.identity);
                newNode.Initialize(this, i, j);

                grid[i, j] = newNode;
            }
        }
    }

    public PathfindingNode GetNode(int xPos, int yPos)
    {
        if (xPos < 0 || xPos >= width || yPos < 0 || yPos >= height) return null;
        return grid[xPos, yPos];
    }

}
