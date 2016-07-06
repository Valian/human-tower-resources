using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class GraphGenerator
{
    private static readonly Random Random = new Random();

    public static Vector3[] GenerateGraphOnCuboid(Vector3 location, Vector3 size, int partsCount, out int[][] edges,
        float randomizationPercentage = 0f)
    {
        List<Vector3> nodes = new List<Vector3>();
        List<int[]> edgesList = new List<int[]>();
        Vector3[,,] nodesMesh = new Vector3[partsCount, partsCount, partsCount];

        Vector3 partSize = size / (partsCount - 1);

        for (int i = 0; i < partsCount; i++)
        {
            for (int j = 0; j < partsCount; j++)
            {
                for (int k = 0; k < partsCount; k++)
                {
                    Vector3 randomVector3 = GetRandomVector3(randomizationPercentage, partSize);

                    Vector3 node = new Vector3(location.x + i * partSize.x, location.y + j * partSize.y,
                        location.x + k * partSize.z) + randomVector3;

                    nodesMesh[i, j, k] = node;
                    nodes.Add(node);
                }
            }
        }

        for (int i = 0; i < partsCount; i++)
        {
            for (int j = 0; j < partsCount; j++)
            {
                for (int k = 0; k < partsCount; k++)
                {
                    if (i + 1 < partsCount)
                    {
                        edgesList.Add(new[] { nodes.IndexOf(nodesMesh[i, j, k]), nodes.IndexOf(nodesMesh[i + 1, j, k]) });
                    }
                    if (j + 1 < partsCount)
                    {
                        edgesList.Add(new[] { nodes.IndexOf(nodesMesh[i, j, k]), nodes.IndexOf(nodesMesh[i, j + 1, k]) });
                    }
                    if (k + 1 < partsCount)
                    {
                        edgesList.Add(new[] { nodes.IndexOf(nodesMesh[i, j, k]), nodes.IndexOf(nodesMesh[i, j, k + 1]) });
                    }
                }
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }

    /// <summary>
    ///     Floors are defined as horizontal parallel panes evenly distibuted through the <paramref name="coneHeight" />.
    ///     The nodes are distributed evenly on circles obtained as the instersections of the panes and the cone of given
    ///     parameters.
    ///     <paramref name="coneLocation" /> is defined at cone base center.
    /// </summary>
    public static Vector3[] GenerateGraphOnCone(Vector3 coneLocation, float coneHeight, float coneRadius,
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

            currentNodesFloor = GenerateNodeLocationsOnCircle(coneLocation.x, floorY, coneLocation.z, floorRadius,
                floorsNodesCounts[floorNo], randomizationPercentage, partSize);
            nodes.AddRange(currentNodesFloor);

            edgesList.AddRange(GetEdgesOnFloor(currentNodesFloor, nodes));

            if (floorNo > 0)
            {
                edgesList.AddRange(GetEdgesBetweenTwoFloors(lastNodesFloor, currentNodesFloor, nodes));
            }
        }

        edges = edgesList.ToArray();
        return nodes.ToArray();
    }

    private static Vector3 GetRandomVector3(float randomizationPercentage, Vector3 partSize)
    {
        Vector3 random =
            new Vector3(GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.x / 2,
                GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.y / 2,
                GetRandomSign() * (float)Random.NextDouble() * randomizationPercentage * partSize.z / 2);
        return random;
    }

    private static int GetRandomSign()
    {
        int sign = Random.Next(-1, 1);
        return sign;
    }

    private static Vector3[] GenerateNodeLocationsOnCircle(float x, float y, float z, float radius, int nodesCount,
        float randomizationPercentage, Vector3 partSize)
    {
        Vector3[] locations = new Vector3[nodesCount];
        double angleFraction = 2 * Math.PI / nodesCount;

        for (int node = 0; node < nodesCount; node++)
        {
            double currentAngle = node * angleFraction;
            Vector3 randomVector3 = GetRandomVector3(randomizationPercentage, partSize);
            locations[node] = GetLocationFromRadiusAndAngle(x, y, z, radius, currentAngle) + randomVector3;
        }

        return locations;
    }

    private static Vector3 GetLocationFromRadiusAndAngle(float x, float y, float z, float radius, double angle)
    {
        float newX = x + (float) (radius * Math.Cos(angle));
        float newZ = z + (float) (radius * Math.Sin(angle));

        return new Vector3(newX, y, newZ);
    }

    private static List<int[]> GetEdgesBetweenTwoFloors(Vector3[] floorA, Vector3[] floorB, List<Vector3> nodes)
    {
        List<int[]> edges = new List<int[]>();

        foreach (Vector3 nodeA in floorA)
        {
            int nodeAId = nodes.IndexOf(nodeA);
            foreach (Vector3 nodeB in floorB)
            {
                int nodeBId = nodes.IndexOf(nodeB);
                edges.Add(new[] { nodeAId, nodeBId });
            }
        }

        return edges;
    }

    private static List<int[]> GetEdgesOnFloor(Vector3[] floor, List<Vector3> nodes)
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
}
