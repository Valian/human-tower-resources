using UnityEngine;

public class SphericGraphProperties : IGraphProperties
{
    public Vector3 Location { get; set; }

    public float Radius { get; set; }

    public int FloorsCount { get; set; }

    public int[] FloorsNodesCounts { get; set; }

    public float RandomizationPercentage { get; set; }

    public float EdgesProbability { get; set; }
}
