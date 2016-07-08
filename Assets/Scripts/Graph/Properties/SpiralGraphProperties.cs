using UnityEngine;

public class SpiralGraphProperties : IGraphProperties
{
    public Vector3 Location { get; set; }

    public float Height { get; set; }

    public float BaseRadius { get; set; }

    public int FloorsCount { get; set; }

    public int[] FloorsNodesCounts { get; set; }

    public float RandomizationPercentage { get; set; }

    public float EdgesProbability { get; set; }
}
