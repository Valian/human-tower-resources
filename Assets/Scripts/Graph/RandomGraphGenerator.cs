using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomGraphGenerator : IGraphGenerator
{
    public Vector3[] GenerateGraph(IGraphProperties graphProperties, out int[][] edges)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();

        RandomGraphProperties randomGraphProperties = graphProperties as RandomGraphProperties;;

        GenerateNodes(randomGraphProperties.Location, randomGraphProperties.Size, randomGraphProperties.NodesCount,
            nodes);
        GenerateEdges(randomGraphProperties.NodesCount, randomGraphProperties.EdgesProbability, edgesList);

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }

    private static void GenerateNodes(Vector3 location, Vector3 cubeSize, int nodesCount, List<Vector3> nodes)
    {
        for (int i = 0; i < nodesCount; i++)
        {
            nodes.Add(GraphGeneratorHelper.GetRandomCartesianCoordinates(cubeSize));
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
