using UnityEngine;


public class Edge : MonoBehaviour
{
    public void InitEdge(Node from, Node to)
    {
        transform.position = (from.transform.position + to.transform.position) / 2;
        InitLine(from.transform.position, to.transform.position);
    }

    private void InitLine(Vector3 from, Vector3 to)
    {
        var dotLine = GetComponentInChildren<DotLine>();
        dotLine.Init(from, to, this);
    }
}
