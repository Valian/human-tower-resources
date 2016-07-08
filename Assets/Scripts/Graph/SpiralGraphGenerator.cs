using UnityEngine;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;

public class SpiralGraphGenerator : IGraphGenerator
{
    public Vector3[] GenerateGraph(IGraphProperties graphProperties, out int[][] edges)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();
        Vector3[] lastNodesFloor = null;
        Vector3[] currentNodesFloor = null;
        SpiralGraphProperties spiralGraphProperties = graphProperties as SpiralGraphProperties;

        float topY = spiralGraphProperties.Location.y + spiralGraphProperties.Height;
        float floorHeight = spiralGraphProperties.Height / (spiralGraphProperties.FloorsCount - 1);

        for (int floorNo = 0; floorNo < spiralGraphProperties.FloorsCount; floorNo++)
        {
            float floorY = spiralGraphProperties.Location.y + floorNo * floorHeight;
            float newConeHeight = topY - floorY;
            float floorRadius = spiralGraphProperties.BaseRadius * newConeHeight / spiralGraphProperties.Height;

            if (floorNo > 0)
            {
                lastNodesFloor = new Vector3[currentNodesFloor.Length];
                currentNodesFloor.CopyTo(lastNodesFloor, 0);
            }

            Vector3 partSize = new Vector3(floorRadius / 2, floorHeight, floorRadius / 2);
            Vector3 floorOrigin = new Vector3(spiralGraphProperties.Location.x, floorY, spiralGraphProperties.Location.z);

            currentNodesFloor = GraphGeneratorHelper.GenerateNodeLocationsOnCircle(floorOrigin, floorRadius,
                spiralGraphProperties.FloorsNodesCounts[floorNo], spiralGraphProperties.RandomizationPercentage, partSize);
            nodes.AddRange(currentNodesFloor);

            edgesList.AddRange(GraphGeneratorHelper.GetEdgesInCircle(currentNodesFloor, nodes, true));

            if (floorNo > 0)
            {
                edgesList.Add(new[]
                {
                    nodes.IndexOf(lastNodesFloor[lastNodesFloor.Length - 1]),
                    nodes.IndexOf(currentNodesFloor[0])
                });
                edgesList.AddRange(GraphGeneratorHelper.GetEdgesBetweenTwoSetsAllToAll(lastNodesFloor, currentNodesFloor,
                    nodes, spiralGraphProperties.EdgesProbability));
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }
}
