using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {

    public Node NodeObject;
    public Edge ConnectionObject;

    private Edge[,] connections;

    public void Generate()
    {

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
