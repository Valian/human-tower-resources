/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections;
using System;

public class LightningBolt : MonoBehaviour
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

        float timex = Time.time * speed * 0.1365143f;
        float timey = Time.time * speed * 1.21688f;
        float timez = Time.time * speed * 1;
		
		for (int i=0; i < _particles.Length; i++)
		{
			Vector3 position = Vector3.Lerp(transform.position, target, _oneOverZigs * (float)i);
			Vector3 offset = new Vector3(_noise.Noise(timex + position.x, timex + position.y, timex + position.z),
										_noise.Noise(timey + position.x, timey + position.y, timey + position.z),
										_noise.Noise(timez + position.x, timez + position.y, timez + position.z));
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
        _particleEmitter.emit = false;
        transform.position = from;
        target = to;
        _particleEmitter.Emit(zigs);
        _particles = _particleEmitter.particles;
        _initialized = true;
        _parent = parent;
    }
}