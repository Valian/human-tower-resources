using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    [Range(0, 500)]
    public int MaxHP;

    [Range(0f, 50f)]
    public float Speed;
    
    private int hp;
    private GameObject target;
    private NodeManager nodeManager;

    void Start () {
        nodeManager = GameObject.FindObjectOfType<NodeManager>();
	}
	
	void Update () {
	}
}
