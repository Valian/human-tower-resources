using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class RandomGraphGenerator
{
    public static Vector3[] GenerateGraph(Vector3 location, Vector3 cubeSize, int nodesCount, out int[][] edges,
        float edgesProbability = 0.5f)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();

        GenerateNodes(cubeSize, nodesCount, nodes);
        GenerateEdges(nodesCount, edgesProbability, edgesList);

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }

    private static void GenerateNodes(Vector3 cubeSize, int nodesCount, List<Vector3> nodes)
    {
        for (int i = 0; i < nodesCount; i++)
        {
            nodes.Add(GraphGeneratorHelper.GetRandomVector3(cubeSize));
        }
    }

    private static void GenerateEdges(int nodesCount, float edgesProbability, List<int[]> edgesList)
    {
        Random random = new Random();
        for (int i = 0; i < nodesCount; i++)
        {
            for (int j = 0; j < nodesCount; j++)
            {
                double probability = random.NextDouble();
                if (probability < edgesProbability)
                {
                    edgesList.Add(new[] {i, j});
                }
            }
        }
    }
}
