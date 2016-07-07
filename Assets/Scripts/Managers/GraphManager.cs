using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum GraphType
{
    Conic,
    Cubic,
    Random
}

public class GraphManager : MonoBehaviour
{

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

    private readonly ConicGraphGenerator _conicGraphGenerator = new ConicGraphGenerator();
    private readonly CubicGraphGenerator _cubicGraphGenerator = new CubicGraphGenerator();
    private readonly RandomGraphGenerator _randomGraphGenerator = new RandomGraphGenerator();

    public void Generate()
    {
        int[][] edges = null;
        Vector3[] nodesLocations = null;

        switch (GraphType)
        {
            case GraphType.Conic:
            {
                ConicGraphProperties conicGraphProperties = new ConicGraphProperties
                {
                    Location = transform.position,
                    RandomizationPercentage = RandomizationPercentage,
                    Height = ConeHeight,
                    FloorsCount = ConeFloorCount,
                    BaseRadius = ConeRadius
                };
                nodesLocations = GenerateNodeConicLocations(conicGraphProperties, out edges);
                break;
            }
            case GraphType.Cubic:
            {
                CubicGraphProperties cubicGraphProperties = new CubicGraphProperties
                {
                    RandomizationPercentage = RandomizationPercentage,
                    Location = transform.position,
                    PartsCount = CubePartsCount,
                    Size = CubeSize
                };
                nodesLocations = GenerateNodeCubicLocations(cubicGraphProperties, out edges);
                break;
            }
            case GraphType.Random:
            {
                RandomGraphProperties randomGraphProperties = new RandomGraphProperties
                {
                    Location = transform.position,
                    NodesCount = RandomNodesCount,
                    EdgesProbability = RandomEdgesProbability,
                    Size = RandomCubeSize
                };
                nodesLocations = GenenerateNodeRandomLocations(randomGraphProperties, out edges);
                break;
            }
        }

        InitializeNodes(nodesLocations);
        InitializeEdges(this.nodes, edges);
    }

    public Vector3[] GenerateNodeConicLocations(ConicGraphProperties conicGraphProperties, out int[][] edges)
    {
        int nodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(out nodesCount);
        conicGraphProperties.FloorsNodesCounts = floorNodesCounts;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _conicGraphGenerator.GenerateGraph(conicGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenerateNodeCubicLocations(CubicGraphProperties cubicGraphProperties, out int[][] edges)
    {
        int nodesCount = CubePartsCount * CubePartsCount * CubePartsCount;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _cubicGraphGenerator.GenerateGraph(cubicGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenenerateNodeRandomLocations(RandomGraphProperties randomGraphProperties, out int[][] edges)
    {
        connections = new Edge[RandomNodesCount, RandomNodesCount];
        Vector3[] nodesLocations = _randomGraphGenerator.GenerateGraph(randomGraphProperties, out edges);
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
        for (int dim = 0; dim <= 1; dim++)
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
