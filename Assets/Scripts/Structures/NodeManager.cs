using System.Collections.Generic;
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
    
    public void Generate()
    {
        int allNodesCount;
        int[] floorNodesCounts = GenerateFloorNodesCounts(out allNodesCount);
        connections = new Edge[allNodesCount, allNodesCount];
        int[][] edges;

        Vector3[] nodesLocations = GraphGenerator.GenerateGraphOnCone(transform.position, ConeHeight,
            ConeRadius, FloorCount, floorNodesCounts, out edges);

        Node[] nodes = InitializeNodes(nodesLocations);
        InitializeEdges(nodes, edges);
    }

    public bool IsConnected(int from, int to)
    {
        var edge = getEdge(from, to);
        return edge != null;
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
    
    private Node[] InitializeNodes(Vector3[] nodesLocations)
    {
        Node[] nodes = new Node[nodesLocations.Length];
        for (int nodeId = 0; nodeId < nodesLocations.Length; nodeId++)
        {
            nodes[nodeId] = InstantiateNode(nodeId, nodesLocations[nodeId]);
        }
        return nodes;
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
