using UnityEngine;
using System.Collections.Generic;

public static class ConicGraphGenerator
{
    /// <summary>
    ///     Floors are defined as horizontal parallel panes evenly distibuted through the <paramref name="coneHeight" />.
    ///     The nodes are distributed evenly on circles obtained as the instersections of the panes and the cone of given
    ///     parameters.
    ///     <paramref name="coneLocation" /> is defined at cone base center.
    /// </summary>
    public static Vector3[] GenerateGraph(Vector3 coneLocation, float coneHeight, float coneRadius,
        int floorsCount, int[] floorsNodesCounts, out int[][] edges, float randomizationPercentage = 0f)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();
        Vector3[] lastNodesFloor = null;
        Vector3[] currentNodesFloor = null;

        float coneTopY = coneLocation.y + coneHeight;
        float floorHeight = coneHeight / (floorsCount - 1);

        for (int floorNo = 0; floorNo < floorsCount; floorNo++)
        {
            float floorY = coneLocation.y + floorNo * floorHeight;
            float newConeHeight = coneTopY - floorY;
            float floorRadius = coneRadius * newConeHeight / coneHeight;

            if (floorNo > 0)
            {
                lastNodesFloor = new Vector3[currentNodesFloor.Length];
                currentNodesFloor.CopyTo(lastNodesFloor, 0);
            }

            Vector3 partSize = new Vector3(floorRadius / 2, floorHeight, floorRadius / 2);

            currentNodesFloor = GraphGeneratorHelper.GenerateNodeLocationsOnCircle(coneLocation.x, floorY, coneLocation.z, floorRadius,
                floorsNodesCounts[floorNo], randomizationPercentage, partSize);
            nodes.AddRange(currentNodesFloor);

            edgesList.AddRange(GraphGeneratorHelper.GetEdgesOnFloor(currentNodesFloor, nodes));

            if (floorNo > 0)
            {
                edgesList.AddRange(GraphGeneratorHelper.GetEdgesBetweenTwoFloors(lastNodesFloor, currentNodesFloor, nodes));
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }
}
