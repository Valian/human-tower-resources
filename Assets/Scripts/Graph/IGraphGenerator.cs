using UnityEngine;

public interface IGraphGenerator
{
    Vector3[] GenerateGraph(IGraphProperties graphProperties, out int[][] edges);
}
