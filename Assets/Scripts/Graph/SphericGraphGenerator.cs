using UnityEngine;
using System.Collections.Generic;

public class SphericGraphGenerator : IGraphGenerator
{
    public Vector3[] GenerateGraph(IGraphProperties graphProperties, out int[][] edges)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();
        Vector3[] lastNodesFloor = null;
        Vector3[] currentNodesFloor = null;
        SphericGraphProperties sphericGraphProperties = graphProperties as SphericGraphProperties;
        
        float radiusPart = sphericGraphProperties.Radius / sphericGraphProperties.FloorsCount;
        Vector3 partSize = new Vector3(radiusPart / 2, radiusPart / 2, radiusPart / 2);

        for (int floorNo = 0; floorNo < sphericGraphProperties.FloorsCount; floorNo++)
        {
            float radiusFrom = sphericGraphProperties.Location.y + floorNo * radiusPart;
            float radiusTo = sphericGraphProperties.Location.y + (floorNo + 1) * radiusPart;

            if (floorNo > 0)
            {
                lastNodesFloor = new Vector3[currentNodesFloor.Length];
                currentNodesFloor.CopyTo(lastNodesFloor, 0);
            }

            currentNodesFloor = GraphGeneratorHelper.GenerateNodeLocationsInSphereSlice(
                sphericGraphProperties.Location, radiusFrom, radiusTo, sphericGraphProperties.FloorsNodesCounts[floorNo],
                sphericGraphProperties.RandomizationPercentage, partSize);
            nodes.AddRange(currentNodesFloor);

            edgesList.AddRange(GraphGeneratorHelper.GetEdgesInCircle(currentNodesFloor, nodes));

            if (floorNo > 0)
            {
                edgesList.AddRange(GraphGeneratorHelper.GetEdgesBetweenTwoSetsAllToAll(lastNodesFloor, currentNodesFloor,
                    nodes, sphericGraphProperties.EdgesProbability, 2 * radiusPart));
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }
}
