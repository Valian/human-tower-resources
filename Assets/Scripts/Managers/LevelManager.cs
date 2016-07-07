using UnityEngine;
using System.Collections.Generic;

public static class LevelManager
{
    public static int MaxLevelNo { get { return _levels.Count; } }

    public class LevelDefinition
    {
        public GraphType        GraphType { get; set; }
        public IGraphProperties GraphSettings { get; set; }
        public int              EnemiesCount { get; set; }
    }

    public static LevelDefinition GetLevelDefinition(int levelNo)
    {
        if (levelNo < 1 || levelNo >= MaxLevelNo)
        {
            levelNo = 1;
        }
        return _levels[levelNo - 1];
    }

    public static void InitiateLevels(Vector3 seedPosition)
    {
        _levels = new List<LevelDefinition> {
            new LevelDefinition
            {
                GraphType = GraphType.Cubic,
                GraphSettings = new CubicGraphProperties
                {
                    Location = seedPosition,
                    PartsCount = 2,
                    RandomizationPercentage = 1,
                    Size = new Vector3(100, 75, 75)
                },
                EnemiesCount = 1
            },
            new LevelDefinition
            {
                GraphType = GraphType.Cubic,
                GraphSettings = new CubicGraphProperties
                {
                    Location = seedPosition,
                    PartsCount = 3,
                    RandomizationPercentage = 1,
                    Size = new Vector3(200, 150, 150)
                },
                EnemiesCount = 3
            },
            new LevelDefinition
            {
                GraphType = GraphType.Conic,
                GraphSettings = new ConicGraphProperties
                {
                    Location = seedPosition,
                    RandomizationPercentage = 1,
                    BaseRadius = 100,
                    FloorsCount = 2,
                    Height = 200
                },
                EnemiesCount = 2
            },
            new LevelDefinition
            {
                GraphType = GraphType.Conic,
                GraphSettings = new ConicGraphProperties
                {
                    Location = seedPosition,
                    RandomizationPercentage = 1,
                    BaseRadius = 200,
                    FloorsCount = 5,
                    Height = 400
                },
                EnemiesCount = 4
            },
            new LevelDefinition
            {
                GraphType = GraphType.Random,
                GraphSettings = new RandomGraphProperties
                {
                    Location = seedPosition,
                    EdgesProbability = 0.8f,
                    NodesCount = 5,
                    Size = new Vector3(400, 400, 400)
                },
                EnemiesCount = 5
            }
        };
    }

    private static List<LevelDefinition> _levels;
}
