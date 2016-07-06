using UnityEngine;
using System.Collections;
using System;

public class Dot : ScoreBall
{
    private int             _id;
    private DotProperties   _props;
    private Transform       _transform;

    public override void Start()
    {
        base.Start();
        _transform = transform;
    }

    void Update()
    {
        Bounce();
    }

    private void Bounce()
    {
        float time = Time.time * _props.Speed;

        Vector3 position = Vector3.Lerp(_props.Parent.transform.position, _props.Target, _props.Interval * (float)_id);
        Vector3 offset = new Vector3((float)Math.Sin(time + position.x),
                                    (float)Math.Sin(time + position.y),
                                    (float)Math.Sin(time + position.z));

        position += (offset * _props.Scale * ((float)_props.DotsCount / 2 * _props.Interval));
        _transform.position = position;
    }

    public void Init(int id, DotProperties props)
    {
        _id = id;
        _props = props;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Collect();
        }
    }
}

