using UnityEngine;
using System.Collections.Generic;

public class ConicGraphGenerator : IGraphGenerator
{
    public Vector3[] GenerateGraph(IGraphProperties graphProperties, out int[][] edges)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();
        Vector3[] lastNodesFloor = null;
        Vector3[] currentNodesFloor = null;
        ConicGraphProperties conicGraphProperties = graphProperties as ConicGraphProperties;

        float topY = conicGraphProperties.Location.y + conicGraphProperties.Height;
        float floorHeight = conicGraphProperties.Height / (conicGraphProperties.FloorsCount - 1);

        for (int floorNo = 0; floorNo < conicGraphProperties.FloorsCount; floorNo++)
        {
            float floorY = conicGraphProperties.Location.y + floorNo * floorHeight;
            float newConeHeight = topY - floorY;
            float floorRadius = conicGraphProperties.BaseRadius * newConeHeight / conicGraphProperties.Height;

            if (floorNo > 0)
            {
                lastNodesFloor = new Vector3[currentNodesFloor.Length];
                currentNodesFloor.CopyTo(lastNodesFloor, 0);
            }

            Vector3 partSize = new Vector3(floorRadius / 2, floorHeight, floorRadius / 2);
            Vector3 floorOrigin = new Vector3(conicGraphProperties.Location.x, floorY, conicGraphProperties.Location.z);

            currentNodesFloor = GraphGeneratorHelper.GenerateNodeLocationsOnCircle(floorOrigin, floorRadius,
                conicGraphProperties.FloorsNodesCounts[floorNo], conicGraphProperties.RandomizationPercentage, partSize);
            nodes.AddRange(currentNodesFloor);

            edgesList.AddRange(GraphGeneratorHelper.GetEdgesInCircle(currentNodesFloor, nodes));

            if (floorNo > 0)
            {
                edgesList.AddRange(GraphGeneratorHelper.GetEdgesBetweenTwoSetsAllToAll(lastNodesFloor, currentNodesFloor, nodes));
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }
}
