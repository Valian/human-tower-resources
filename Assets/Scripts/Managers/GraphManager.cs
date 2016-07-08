using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum GraphType
{
    Conic,
    Cubic,
    Random,
    Spheric,
    Spiral
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

    public float SpiralHeight;
    public float SpiralRadius;
    public int SpiralFloorCount;
    [Range(0f, 1f)]
    public float SpiralEdgesProbability;

    private Edge[,] connections;

    private readonly ConicGraphGenerator _conicGraphGenerator = new ConicGraphGenerator();
    private readonly CubicGraphGenerator _cubicGraphGenerator = new CubicGraphGenerator();
    private readonly RandomGraphGenerator _randomGraphGenerator = new RandomGraphGenerator();
    private readonly SphericGraphGenerator _sphericGraphGenerator = new SphericGraphGenerator();
    private readonly SpiralGraphGenerator _spiralGraphGenerator = new SpiralGraphGenerator();

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
            case GraphType.Spiral:
            {
                SpiralGraphProperties spiralGraphProperties = (SpiralGraphProperties) properties ??
                                                            new SpiralGraphProperties
                                                            {
                                                                Location = transform.position,
                                                                RandomizationPercentage = RandomizationPercentage,
                                                                Height = SpiralHeight,
                                                                FloorsCount = SpiralFloorCount,
                                                                BaseRadius = SpiralRadius,
                                                                EdgesProbability = SpiralEdgesProbability
                                                            };
                nodesLocations = GenerateNodeSpiralLocations(spiralGraphProperties, out edges);
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
        if (conicGraphProperties.FloorsNodesCounts == null)
        {
            int[] floorNodesCounts = GraphGeneratorHelper.GenerateFloorNodesCounts(conicGraphProperties.FloorsCount, -1,
                conicGraphProperties.FloorsCount);
            conicGraphProperties.FloorsNodesCounts = floorNodesCounts;
        }
        int nodesCount = conicGraphProperties.FloorsNodesCounts.Sum(a => a);
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
        if (sphericGraphProperties.FloorsNodesCounts == null)
        {
            int[] floorNodesCounts = GraphGeneratorHelper.GenerateFloorNodesCounts(2, 2,
                sphericGraphProperties.FloorsCount);
            sphericGraphProperties.FloorsNodesCounts = floorNodesCounts;
        }
        int nodesCount = sphericGraphProperties.FloorsNodesCounts.Sum(a => a);
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _sphericGraphGenerator.GenerateGraph(sphericGraphProperties, out edges);
        return nodesLocations;
    }

    public Vector3[] GenerateNodeSpiralLocations(SpiralGraphProperties spiralGraphProperties, out int[][] edges)
    {
        if (spiralGraphProperties.FloorsNodesCounts == null)
        {
            int[] floorNodesCounts = GraphGeneratorHelper.GenerateFloorNodesCounts(spiralGraphProperties.FloorsCount, -1,
                spiralGraphProperties.FloorsCount);
            spiralGraphProperties.FloorsNodesCounts = floorNodesCounts;
        }
        int nodesCount = spiralGraphProperties.FloorsNodesCounts.Sum(a => a);
        connections = new Edge[nodesCount, nodesCount];
        Vector3[] nodesLocations = _spiralGraphGenerator.GenerateGraph(spiralGraphProperties, out edges);
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

    public List<int>[] GetPath(int start)
    {
        int len = connections.GetLength(0);
        int[] dist = new int[len];
        List<int>[] path = new List<int>[len];
        List<int> queue = new List<int>();

        for (int i = 0; i<len; i++)
        {
            dist[i] = int.MaxValue;
            queue.Add(i);
            path[i] = new List<int>();
        }
        dist[start] = 0;

        while(queue.Count > 0)
        {
            queue.Sort((x, y) => dist[x] - dist[y]);
            int u = GetNextVertex(queue, dist);
            for(int v = 0; v < len; v++)
            {
                if(connections[u,v] != null)
                {
                    if(dist[v] > dist[u] + (connections[u,v] != null ? 1 : 0))
                    {
                        dist[v] = dist[u] + (connections[u, v] != null ? 1 : 0);
                        path[v].Add(u);
                        queue.Add(v);
                    }
                }
            }
        }
        return path;
    }
    private int GetNextVertex(List<int> queue, int[] dist)
    {
        int min = int.MaxValue;
        int Vertex = -1;

        foreach (int j in queue)
        {
            if (dist[j] <= min)
            {
                min = dist[j];
                Vertex = j;
            }
        }
        queue.Remove(Vertex);
        return Vertex;
    }
}
