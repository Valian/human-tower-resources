using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class GraphGeneratorHelper
{
    private static readonly Random Random = new Random();

    public static Vector3 GetRandomCartesianCoordinates(Vector3 partSize, float randomizationPercentage = 1f)
    {
        Vector3 random =
            new Vector3(GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.x / 2,
                GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.y / 2,
                GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.z / 2);
        return random;
    }

    public static void GetRandomSphericCoordinates(float radiusFrom, float radiusTo, out float radius,
        out double azimuthalAngle, out double polarAngle, float randomizationPercentage = 1f)
    {
        float radiusDiff = radiusTo - radiusFrom;
        radius = (float) Random.NextDouble() * radiusDiff + radiusFrom;
        azimuthalAngle = Random.NextDouble() * Math.PI * 2;
        polarAngle = Random.NextDouble() * Math.PI * 2;
    }

    public static Vector3[] GenerateNodeLocationsOnCircle(Vector3 origin, float radius, int nodesCount,
        float randomizationPercentage, Vector3 partSize)
    {
        Vector3[] locations = new Vector3[nodesCount];
        double angleFraction = 2 * Math.PI / nodesCount;

        for (int node = 0; node < nodesCount; node++)
        {
            double currentAngle = node * angleFraction;
            Vector3 randomVector3 = GetRandomCartesianCoordinates(partSize, randomizationPercentage);
            locations[node] = GetCartesianFromSphericalCoordinates(origin, radius, currentAngle) + randomVector3;
        }

        return locations;
    }

    public static Vector3[] GenerateNodeLocationsInSphereSlice(Vector3 origin, float radiusFrom, float radiusTo,
        int nodesCount, float randomizationPercentage, Vector3 partSize)
    {
        Vector3[] locations = new Vector3[nodesCount];

        for (int node = 0; node < nodesCount; node++)
        {
            float radius;
            double azimuthalAngle, polarAngle;
            GetRandomSphericCoordinates(radiusFrom, radiusTo, out radius, out azimuthalAngle, out polarAngle,
                randomizationPercentage);
            Vector3 randomVector3 = GetRandomCartesianCoordinates(partSize, randomizationPercentage);
            locations[node] = GetCartesianFromSphericalCoordinates(origin, radius, azimuthalAngle, polarAngle) + randomVector3;
        }

        return locations;
    }

    public static List<int[]> GetEdgesBetweenTwoFloors(Vector3[] floorA, Vector3[] floorB, List<Vector3> nodes,
        float edgesProbability = 1f, float distance = float.MaxValue)
    {
        List<int[]> edges = new List<int[]>();

        foreach (Vector3 nodeA in floorA)
        {
            int nodeAId = nodes.IndexOf(nodeA);
            foreach (Vector3 nodeB in floorB)
            {
                double probability = Random.NextDouble();
                if (probability < edgesProbability && (nodeA - nodeB).magnitude < distance)
                {
                    int nodeBId = nodes.IndexOf(nodeB);
                    edges.Add(new[] { nodeAId, nodeBId });
                }

            }
        }

        return edges;
    }

    public static List<int[]> GetEdgesOnFloor(Vector3[] floor, List<Vector3> nodes)
    {
        List<int[]> edges = new List<int[]>();

        if (floor.Length > 2)
        {
            int nodesCount = floor.Length;
            for (int nodeNo = 0; nodeNo < nodesCount; nodeNo++)
            {
                edges.Add(new[]
                {
                    nodes.IndexOf(floor[nodeNo]),
                    nodeNo == nodesCount - 1 ? nodes.IndexOf(floor[0]) : nodes.IndexOf(floor[nodeNo + 1])
                });
            }
        }

        return edges;
    }

    private static int GetRandomSign()
    {
        int sign = Random.Next(-1, 1);
        return sign;
    }

    private static Vector3 GetCartesianFromSphericalCoordinates(Vector3 origin, float radius, double azimuthalAngle,
        double polarAngle = Math.PI / 2)
    {
        float newX = origin.x + (float) (radius * Math.Sin(polarAngle) * Math.Cos(azimuthalAngle));
        float newY = origin.y + (float) (radius * Math.Cos(polarAngle));
        float newZ = origin.z + (float) (radius * Math.Sin(polarAngle) * Math.Sin(azimuthalAngle));

        return new Vector3(newX, newY, newZ);
    }
}
