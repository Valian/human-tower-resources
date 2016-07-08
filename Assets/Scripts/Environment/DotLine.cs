using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DotLine : MonoBehaviour
{
	public Vector3      target;
    public int          DotsCount = 2;
    public GameObject   DotPrefab;
    public GameObject   PowerDotPrefab;
    public float        speed = 1f;
	public float        scale = 1f;
    
	private float           _oneOverZigs;	
    private LineRenderer    _lineRenderer;
    private bool            _initialized;
    private Edge            _parent;
    private List<ScoreBall>       _dots;

	void Start()
	{ }

    public void Init(Vector3 from, Vector3 to, Edge parent, bool hasPowerDot)
    {
        _oneOverZigs = 1f / (float)DotsCount;
        _lineRenderer = GetComponent<LineRenderer>();
        transform.position = from;
        target = to;
        _initialized = true;
        _parent = parent;
        _lineRenderer.SetPosition(0, from);
        _lineRenderer.SetPosition(1, to);
        SpawnDots(hasPowerDot);
    }

    private void SpawnDots(bool hasPowerDot)
    {
        _dots = new List<ScoreBall>(DotsCount);
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
            ScoreBall newDot = null;
            if(hasPowerDot && i == DotsCount / 2)
            {
                newDot = Instantiate(PowerDotPrefab).GetComponent<PowerDot>();
            } else
            {
                newDot = Instantiate(DotPrefab).GetComponent<Dot>();
            }
            ((Dot)newDot).Init(i, prop);
            newDot.transform.parent = transform;
            _dots.Add(newDot);
        }
    }

}