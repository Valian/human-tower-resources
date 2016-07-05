using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Range(0, 500)]
    public int MaxHP;

    [Range(0f, 50f)]
    public float Speed;
    
    private int hp;
    private GameObject target;   
        
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
