using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DotLine : MonoBehaviour
{
	public Vector3      target;
    public int          DotsCount = 2;
    public GameObject   DotPrefab;
	public float        speed = 1f;
	public float        scale = 1f;
    
	private float           _oneOverZigs;	
    private LineRenderer    _lineRenderer;
    private bool            _initialized;
    private Edge            _parent;
    private List<Dot>       _dots;

	void Start()
	{
	}

    public void Init(Vector3 from, Vector3 to, Edge parent)
    {
        _oneOverZigs = 1f / (float)DotsCount;
        _lineRenderer = GetComponent<LineRenderer>();
        transform.position = from;
        target = to;
        _initialized = true;
        _parent = parent;
        _lineRenderer.SetPosition(0, from);
        _lineRenderer.SetPosition(1, to);
        SpawnDots();
    }

    private void SpawnDots()
    {
        _dots = new List<Dot>(DotsCount);
        var prop = new DotProperties
        {
            Target = target,
            Interval = _oneOverZigs,
            DotsCount = DotsCount,
            Scale = scale,
            Speed = speed,
            Parent = this
        };
        for (int i = 0; i < DotsCount; i++)
        {
            var newDot = Instantiate(DotPrefab).GetComponent<Dot>();
            newDot.Init(i, prop);
            newDot.transform.parent = transform;
            _dots.Add(newDot);
        }
    }

}