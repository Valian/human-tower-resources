using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum GraphType
{
    Cone,
    Cubic,
    Random
}

public class NodeManager : MonoBehaviour {

    public Node NodeObjectPrefab;
    public Edge EdgeNodePrefab;
    public PlayerLinearMovement PlayerPrefab;

    public GraphType GraphType;
    [Range(0f, 1f)]
    public float RandomizationPercentage;

    public float ConeHeight;
    public float ConeRadius;
    public int ConeFloorCount;

    public Vector3 CubeSize;
    public int CubePartsCount;

    public Vector3 RandomCubeSize;
    public int RandomNodesCount;
    [Range(0f, 1f)]
    public float RandomEdgesProbability;

    private Edge[,] connections;
    private Node[] nodes;
    
    public void Generate()
    {
        int[][] edges = null;
        Vector3[] nodesLocations = null;

        switch (GraphType)
        {
            case GraphType.Cone:
            {
                nodesLocations = GenerateNodeConicLocations(out edges);
                break;
            }
            case GraphType.Cubic:
            {
                nodesLocations = GenerateNodeCubicLocations(out edges);
                break;
            }
            case GraphType.Random:
            {
                nodesLocations = GenenerateNodeRandomLocations(out edges);
                break;
            }
        }

        InitializeNodes(nodesLocations);
        InitializeEdges(this.nodes, edges);
    }

    private Vector3[] GenerateNodeConicLocations(out int[][] edges)
    {
        int nodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(out nodesCount);
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = ConicGraphGenerator.GenerateGraph(transform.position, ConeHeight, ConeRadius,
            ConeFloorCount, floorNodesCounts, out edges, RandomizationPercentage);
        return nodesLocations;
    }

    private Vector3[] GenerateNodeCubicLocations(out int[][] edges)
    {
        int nodesCount = CubePartsCount * CubePartsCount * CubePartsCount;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = CuboidGraphGenerator.GenerateGraph(transform.position, CubeSize, CubePartsCount,
            out edges, RandomizationPercentage);
        return nodesLocations;
    }

    private Vector3[] GenenerateNodeRandomLocations(out int[][] edges)
    {
        connections = new Edge[RandomNodesCount, RandomNodesCount];
        Vector3[] nodesLocations = RandomGraphGenerator.GenerateGraph(transform.position, RandomCubeSize,
            RandomNodesCount, out edges, RandomEdgesProbability);
        return nodesLocations;
    }

    public bool IsConnected(int from, int to)
    {
        var edge = getEdge(from, to);
        return edge != null;
    }

    public ICollection<Node> GetNeightbours(Node node)
    {
        var result = new HashSet<Node>();
        for(int dim = 0; dim <= 1; dim++)
        {
            for (int i = 0; i < connections.GetLength(dim); i++)
            {
                if (node.NodeId != i && IsConnected(node.NodeId, i))
                {
                    result.Add(nodes[i]);
                }
            }
        }
        return result;
    }

    private int[] GenerateFloorNodesCounts(out int nodesCount)
    {
        int[] floorNodesCounts = new int[ConeFloorCount];
        for (int i = 0; i < ConeFloorCount; i++)
        {
            floorNodesCounts[i] = ConeFloorCount - i;
        }
        nodesCount = floorNodesCounts.Sum(a => a);
        return floorNodesCounts;
    }
    
    private void InitializeNodes(Vector3[] nodesLocations)
    {
        Node[] nodes = new Node[nodesLocations.Length];
        for (int nodeId = 0; nodeId < nodesLocations.Length; nodeId++)
        {
            nodes[nodeId] = InstantiateNode(nodeId, nodesLocations[nodeId]);
        }
        this.nodes = nodes;
    }

    private void InitializeEdges(Node[] nodes, int[][] edges)
    {
        foreach (int[] edge in edges)
        {
            int from = edge[0];
            int to = edge[1];
            InstantiateEdge(nodes[from], nodes[to]);
        }
    }

    private Node InstantiateNode(int id, Vector3 location)
    {
        Node node = Instantiate(NodeObjectPrefab);
        node.transform.parent = transform;
        node.InitNode(id, location);
        return node;
    }

    private Edge InstantiateEdge(Node from, Node to)
    {
        Edge edge = Instantiate(EdgeNodePrefab);
        edge.transform.parent = transform;
        edge.InitEdge(from, to);
        connections[from.NodeId, to.NodeId] = connections[to.NodeId, from.NodeId] = edge;
        return edge;
    }

    private Edge getEdge(int from, int to)
    {
        if (connections == null) return null;
        var conn = connections[from, to];
        if (conn == null)
        {
            conn = connections[to, from];
        }
        return conn;
    }
}
