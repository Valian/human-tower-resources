﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum GraphType
{
    Conic,
    Cubic,
    Random,
    Spheric
}

public class GraphManager : MonoBehaviour
{

    public Node NodeObjectPrefab;
    public Edge EdgeNodePrefab;
    public PlayerLinearMovement PlayerPrefab;
    public Node[] Nodes { get; private set; }

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

    public int SphereFloorsCount;
    [Range(0f, 1f)]
    public float SphereEdgesProbability;
    public float SphereRadius;
    
    private Edge[,] connections;

    private readonly ConicGraphGenerator _conicGraphGenerator = new ConicGraphGenerator();
    private readonly CubicGraphGenerator _cubicGraphGenerator = new CubicGraphGenerator();
    private readonly RandomGraphGenerator _randomGraphGenerator = new RandomGraphGenerator();
    private readonly SphericGraphGenerator _sphericGraphGenerator = new SphericGraphGenerator();

    public void Generate(GraphType graphType, IGraphProperties properties = null)
    {
        int[][] edges = null;
        Vector3[] nodesLocations = null;

        switch (graphType)
        {
            case GraphType.Conic:
                {
                    ConicGraphProperties conicGraphProperties = (ConicGraphProperties)properties ?? new ConicGraphProperties
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
                    CubicGraphProperties cubicGraphProperties = (CubicGraphProperties)properties ?? new CubicGraphProperties
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
                    RandomGraphProperties randomGraphProperties = (RandomGraphProperties)properties ?? new RandomGraphProperties
                    {
                        Location = transform.position,
                        NodesCount = RandomNodesCount,
                        EdgesProbability = RandomEdgesProbability,
                        Size = RandomCubeSize
                    };
                    nodesLocations = GenenerateNodeRandomLocations(randomGraphProperties, out edges);
                    break;
                }
            case GraphType.Spheric:
            {
                SphericGraphProperties sphericGraphProperties = (SphericGraphProperties) properties ??
                                                                new SphericGraphProperties
                                                                {
                                                                    RandomizationPercentage = RandomizationPercentage,
                                                                    Location = transform.position,
                                                                    FloorsCount = SphereFloorsCount,
                                                                    EdgesProbability = SphereEdgesProbability,
                                                                    Radius = SphereRadius
                                                                };
                nodesLocations = GenerateNodeSphericLocations(sphericGraphProperties, out edges);
                break;
            }
        }

        InitializeNodes(nodesLocations);
        InitializeEdges(this.Nodes, edges);
    }

    public void Generate()
    {
        Generate(GraphType);
    }

    public Vector3[] GenerateNodeConicLocations(ConicGraphProperties conicGraphProperties, out int[][] edges)
    {
        int nodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(conicGraphProperties.FloorsCount, -1,
            conicGraphProperties.FloorsCount, out nodesCount);
        conicGraphProperties.FloorsNodesCounts = floorNodesCounts;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _conicGraphGenerator.GenerateGraph(conicGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenerateNodeCubicLocations(CubicGraphProperties cubicGraphProperties, out int[][] edges)
    {
        int nodesCount = cubicGraphProperties.PartsCount * cubicGraphProperties.PartsCount * cubicGraphProperties.PartsCount;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _cubicGraphGenerator.GenerateGraph(cubicGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenenerateNodeRandomLocations(RandomGraphProperties randomGraphProperties, out int[][] edges)
    {
        connections = new Edge[randomGraphProperties.NodesCount, randomGraphProperties.NodesCount];
        Vector3[] nodesLocations = _randomGraphGenerator.GenerateGraph(randomGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenerateNodeSphericLocations(SphericGraphProperties sphericGraphProperties, out int[][] edges)
    {
        int nodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(2, 2, sphericGraphProperties.FloorsCount, out nodesCount);
        sphericGraphProperties.FloorsNodesCounts = floorNodesCounts;
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _sphericGraphGenerator.GenerateGraph(sphericGraphProperties, out edges);
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
                    result.Add(Nodes[i]);
                }
            }
        }
        return result;
    }
    
    public void DestroyGraph()
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
        children.ForEach(Destroy);
        connections = null;
        Nodes = null;
    }

    public Node GetRandomNode()
    {
        return Nodes[Random.Range(0, Nodes.Length - 1)];
    }

    private int[] GenerateFloorNodesCounts(int start, int step, int floorCount, out int nodesCount)    {
        int[] floorNodesCounts = new int[floorCount];
        for (int i = 0; i < floorCount; i++)
        {
            floorNodesCounts[i] = start;
            start += step;
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
        this.Nodes = nodes;
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
