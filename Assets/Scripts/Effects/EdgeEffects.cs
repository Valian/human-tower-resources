using UnityEngine;
using System.Collections;
using System;

public class EdgeEffects : MonoBehaviour
{
	public Vector3  target;
	public int      zigs = 100;
	public float    speed = 1f;
	public float    scale = 1f;
    public bool     IsTest;
    public ParticleEmitter ActiveEdgeParticles;
    public ParticleEmitter InactiveEdgeParticles;

	private Perlin          _noise;
	private float           _oneOverZigs;	
	private Particle[]      _particles;
    private ParticleEmitter _particleEmitter;
    private LineRenderer    _lineRenderer;
    private bool            _initialized;
    private Edge            _parent;

	void Start()
	{
        if (IsTest)
        {
            Init(transform.position, target, null);
        }
	}
	
	void Update ()
	{
        if ((!_initialized && !IsTest))// || !_parent.IsActive)
            return;

		if (_noise == null)
			_noise = new Perlin();

        float timex = Time.time * speed;
        float timey = Time.time * speed;
        float timez = Time.time * speed;        
		
		for (int i=0; i < _particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, target, _oneOverZigs * (float)i);
			Vector3 offset = new Vector3((float)Math.Sin(timex + position.x),
                                        (float)Math.Sin(timey + position.y),
                                        (float)Math.Sin(timez + position.z));

			position += (offset * scale * ((float)_particles.Length/ 2 * _oneOverZigs));
            
            _particles[i].position = position;
			_particles[i].color = Color.white;
			_particles[i].energy = 1f;
		}

        _particleEmitter.particles = _particles;
	}

    public void Init(Vector3 from, Vector3 to, Edge parent)
    {
        _oneOverZigs = 1f / (float)zigs;
        _particleEmitter = GetComponent<ParticleEmitter>();
        _lineRenderer = GetComponent<LineRenderer>();
        _particleEmitter.emit = false;
        transform.position = from;
        target = to;
        _particleEmitter.Emit(zigs);
        _particles = _particleEmitter.particles;
        _initialized = true;
        _parent = parent;
        _lineRenderer.SetPosition(0, from);
        _lineRenderer.SetPosition(1, to);
    }
}