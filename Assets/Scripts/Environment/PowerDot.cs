﻿using UnityEngine;
using System.Collections;

public class PowerDot : Dot {

    public float ColorLoopDuration = 1;

    private ColorHSV a;
    private ColorHSV b;
    private Material material;

	// Use this for initialization
	public override void Start () {
        base.Start();
        isPowerDot = true;
        a = new ColorHSV(0, 0.8f, 0.8f);
        b = new ColorHSV(1, 0.5f, 0.8f);
        material = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        var progress = Mathf.Sin(Time.realtimeSinceStartup / Mathf.PI * ColorLoopDuration);
        material.color = ColorHSV.ToColor(ColorHSV.Lerp(a, b, progress));
	}
}
