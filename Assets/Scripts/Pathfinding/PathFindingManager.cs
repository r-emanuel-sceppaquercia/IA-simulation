using System;
using UnityEngine;

public enum TypeOfPath
{
    BFS,
    DFS,
    GreedyBFS,
    Dijkstra,
    AStar,
    ThetaStar,
    ThetaBasedOnAStar
}

public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager Instance;

    [Header("Start and goal")]
    public PathfindingNode startNode;
    public PathfindingNode goalNode;

    [Header("Extra settings")]
    [SerializeField] private TypeOfPath typeOfPath;
    [SerializeField] private HeuristicType heuristicType;

    [SerializeField] private bool canMove;

    private DFS dfs;
    private BFS bfs;
    private BFSGreedy bfsGreedy;
    private Dijkstra dijkstra;
    private AStar<PathfindingNode> aStar;
    private ThetaStar thetaStar;

    [Header("Agent")]
    [SerializeField] private PathfindingAgent agent;
    [SerializeField] private LayerMask obstacle;

    public event Action OnResetGrid = delegate { };

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);

        dfs = new DFS();
        bfs = new BFS();
        bfsGreedy = new BFSGreedy();
        dijkstra = new Dijkstra();
        aStar = new AStar<PathfindingNode>();
        thetaStar = new ThetaStar();
    }

    private void Update()
    {
        if (startNode && goalNode && Input.GetKeyDown(KeyCode.Space))
        {
            canMove = false;
            OnResetGrid();

            switch (typeOfPath)
            {
                case TypeOfPath.DFS:
                    StartCoroutine(dfs.CalculateDFSCoroutine(startNode, goalNode));
                    break;
                case TypeOfPath.BFS:
                    StartCoroutine(bfs.CalculateBFSCoroutine(startNode, goalNode));
                    break;
                case TypeOfPath.GreedyBFS:
                    StartCoroutine(bfsGreedy.CalculateBFSGreedyCoroutine(startNode, goalNode));
                    break;
                case TypeOfPath.Dijkstra:
                    StartCoroutine(dijkstra.CalculateDijkstraCoroutine(startNode, goalNode));
                    break;
                case TypeOfPath.AStar:
                    StartCoroutine(aStar.CalculateAStarCoroutine(startNode, goalNode, heuristicType));
                    break;
                case TypeOfPath.ThetaStar:
                    //StartCoroutine(thetaStar.CalculateThetaStarCoroutine(startNode, goalNode, heuristicType, InLineOfSight));
                    break;
                case TypeOfPath.ThetaBasedOnAStar:
                    break;
                default: break;
            }
        }

        if (Input.GetKeyDown(KeyCode.C) && canMove)
        {
            switch (typeOfPath)
            {
                case TypeOfPath.DFS:
                    agent.SetMove(dfs.CalculateDFS(startNode, goalNode));
                    break;
                case TypeOfPath.BFS:
                    agent.SetMove(bfs.CalculateBFS(startNode, goalNode));
                    break;
                case TypeOfPath.GreedyBFS:
                    agent.SetMove(bfsGreedy.CalculateBFSGreedy(startNode, goalNode));
                    break;
                case TypeOfPath.Dijkstra:
                    agent.SetMove(dijkstra.CalculateDijkstra(startNode, goalNode));
                    break;
                case TypeOfPath.AStar:
                    agent.SetMove(aStar.CalculateAStar(startNode, goalNode, heuristicType));
                    break;
                case TypeOfPath.ThetaStar:
                    //agent.SetMove(thetaStar.CalculateThetaStar(startNode, goalNode, heuristicType, InLineOfSight));
                    break;
                case TypeOfPath.ThetaBasedOnAStar:
                default: break;
            }
        }
    }

    public bool InLineOfSight(PathfindingNode start, PathfindingNode end)
    {
        Vector3 dir = end.transform.position - start.transform.position;

        Debug.DrawRay(start.transform.position, dir, Color.red);

        return !Physics.Raycast(start.transform.position, dir.normalized, dir.magnitude, obstacle);
    }
}
