using UnityEngine;

public class RandomGraphProperties : IGraphProperties
{
    public Vector3 Location { get; set; }

    public Vector3 Size { get; set; }

    public int NodesCount { get; set; }

    public float EdgesProbability { get; set; }
}