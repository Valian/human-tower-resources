using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;


public class Edge : MonoBehaviour {
        
    private float currentHP = 0f;
    private List<EdgeEffects> particleEmitters;

    public void InitEdge(Node from, Node to)
    {
        transform.position = (from.transform.position + to.transform.position) / 2;
        InitParticles(from.transform.position, to.transform.position);
    }

    private void InitParticles(Vector3 from, Vector3 to)
    {
        particleEmitters = GetComponentsInChildren<EdgeEffects>().ToList();
        for(int i = 0; i < particleEmitters.Count; i++)
        {
            if (i < particleEmitters.Count / 2)
            {
                particleEmitters[i].Init(from, to, this);
            } else
            {
                particleEmitters[i].Init(to, from, this);
            }
        }
    }  
            
}
