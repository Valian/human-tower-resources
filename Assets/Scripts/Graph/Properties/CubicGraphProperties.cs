using UnityEngine;

public class CubicGraphProperties : IGraphProperties
{
    public Vector3 Location { get; set; }

    public Vector3 Size { get; set; }
    
    public int PartsCount { get; set; }
    
    public float RandomizationPercentage { get; set; }
}
