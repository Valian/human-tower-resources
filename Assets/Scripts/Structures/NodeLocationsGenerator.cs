using System;
using UnityEngine;

public static class NodeLocationsGenerator
{
    /// <summary>
    ///     Floors are defined as horizontal parallel panes evenly distibuted through the <paramref name="coneHeight" />.
    ///     The nodes are distributed evenly on circles obtained as the instersections of the panes and the cone of given
    ///     parameters.
    ///     <paramref name="coneLocation" /> is defined at cone base center.
    /// </summary>
    public static Vector3[][] GenerateNodesLocations(Vector3 coneLocation, float coneHeight, float coneRadius,
        int floorsCount, int[] floorsNodesCounts)
    {
        Vector3[][] locations = new Vector3[floorsCount][];
        float coneTopY = coneLocation.y + coneHeight;
        float floorHeight = coneHeight / floorsCount;

        for (int floor = 0; floor < floorsCount; floor++)
        {
            float floorY = coneLocation.y + floor * floorHeight;
            float newConeHeight = coneTopY - floorY;
            float floorRadius = coneRadius * newConeHeight / coneHeight;
            locations[floor] = GenerateFloorNodesLocations(coneLocation.x, floorY, coneLocation.z, floorRadius,
                floorsNodesCounts[floor]);
        }

        return locations;
    }

    private static Vector3[] GenerateFloorNodesLocations(float x, float y, float z, float radius, int nodesCount)
    {
        Vector3[] locations = new Vector3[nodesCount];

        double angleFraction = 2 * Math.PI / nodesCount;

        for (int node = 0; node < nodesCount; node++)
        {
            double currentAngle = node * angleFraction;
            locations[node] = GetXyzFromRadiusAndAngle(x, y, z, radius, currentAngle);
        }

        return locations;
    }

    private static Vector3 GetXyzFromRadiusAndAngle(float x, float y, float z, float radius, double angle)
    {
        float newX = x + (float) (radius * Math.Cos(angle));
        float newZ = z + (float) (radius * Math.Sin(angle));
        return new Vector3(newX, y, newZ);
    }
}
