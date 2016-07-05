using UnityEngine;
using System.Linq;

public class NodeManager : MonoBehaviour {

    public Node NodeObjectPrefab;
    public Edge EdgeNodePrefab;
    public PlayerLinearMovement PlayerPrefab;
    public float ConeHeight;
    public float ConeRadius;
    public int FloorCount;

    private Edge[,] connections;

    void Start()
    {
        Generate();
        SpawnPlayer();
    }

    public void Generate()
    {
        int allNodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(out allNodesCount);
        connections = new Edge[allNodesCount, allNodesCount];

        Vector3[][] nodesLocations = NodeLocationsGenerator.GenerateNodesLocations(transform.position, ConeHeight,
            ConeRadius, FloorCount, floorNodesCounts);

        InitializeNodesAndEdges(floorNodesCounts, nodesLocations);
    }

    private void SpawnPlayer()
    {
        var player = Instantiate<PlayerLinearMovement>(PlayerPrefab);
        // TODO - jakos sprytniej
        var node = GetComponentInChildren<Node>();
        player.SetPosition(node);
    }

    public bool CanConnect(int from, int to)
    {
        return getEdge(from, to) != null;
    }

    public bool IsConnected(int from, int to)
    {
        var edge = getEdge(from, to);
        return edge != null && edge.IsActive;
    }

    public void FortifyEdge(int from, int to, float hpBuild)
    {
        var edge = getEdge(from, to);
        if(edge)
        {
            edge.FortifyEdge(hpBuild);
        }
    }

    private void InitializeNodesAndEdges(int[] floorNodesCounts, Vector3[][] nodesLocations)
    {
        Node[] lastNodesFloor = null;
        Node[] currentNodesFloor = null;
        int currentNodeId = 0;

        for (int floor = 0; floor < FloorCount; floor++)
        {
            if (floor > 0)
            {
                lastNodesFloor = new Node[currentNodesFloor.Length];
                currentNodesFloor.CopyTo(lastNodesFloor, 0);
            }

            currentNodesFloor = InstantiateNodesOnCurrentFloor(floorNodesCounts, nodesLocations, floor,
                ref currentNodeId);

            InstantiateEdgesOnFloor(currentNodesFloor);

            if (floor > 0)
            {
                InstantiateEdgesBetweenTwoFloors(lastNodesFloor, currentNodesFloor);
            }
        }
    }

    private Node[] InstantiateNodesOnCurrentFloor(int[] floorNodesCounts, Vector3[][] nodesLocations, int floor,
        ref int currentNodeId)
    {
        int nodesCount = floorNodesCounts[floor];
        Node[] currentNodesFloor = new Node[nodesCount];
        for (int nodeNo = 0; nodeNo < nodesCount; nodeNo++)
        {
            currentNodesFloor[nodeNo] = InstantiateNode(currentNodeId, nodesLocations[floor][nodeNo]);
            currentNodeId++;
        }
        return currentNodesFloor;
    }

    private void InstantiateEdgesBetweenTwoFloors(Node[] floorA, Node[] floorB)
    {
        foreach (Node nodeA in floorA)
        {
            foreach (Node nodeB in floorB)
            {
                InstantiateEdge(nodeA, nodeB);
            }
        }
    }

    private void InstantiateEdgesOnFloor(Node[] floor)
    {
        if (floor.Length < 2) return;

        int nodesCount = floor.Length;

        for (int nodeNo = 0; nodeNo < nodesCount; nodeNo++)
        {
            InstantiateEdge(floor[nodeNo], nodeNo == nodesCount - 1 ? floor[0] : floor[nodeNo + 1]);
        }
    }

    private Node InstantiateNode(int id, Vector3 location)
    {
        Node node = Instantiate(NodeObjectPrefab);
        node.transform.parent = transform;
        node.InitNode(id, location);
        return node;
    }

    private void InstantiateEdge(Node from, Node to)
    {
        Edge edge = Instantiate(EdgeNodePrefab);
        edge.transform.parent = transform;
        edge.InitEdge(from, to);
        connections[from.NodeId, to.NodeId] = connections[to.NodeId, from.NodeId] = edge;
    }

    private int[] GenerateFloorNodesCounts(out int nodesCount)
    {
        int[] floorNodesCounts = new int[FloorCount];
        for (int i = 0; i < FloorCount; i++)
        {
            floorNodesCounts[i] = FloorCount - i;
        }
        nodesCount = floorNodesCounts.Sum(a => a);
        return floorNodesCounts;
    }

    private Edge getEdge(int from, int to)
    {
        var conn = connections[from, to];
        if (conn == null)
        {
            conn = connections[to, from];
        }
        return conn;
    }
}
