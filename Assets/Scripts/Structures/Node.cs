using UnityEngine;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour {

    public int NodeId { get; private set; }
    public delegate void NodeHealthChanged(Node node, float damageAmount);
    public event NodeHealthChanged NodeDamaged;
    

    public void TakeDamage(float damagedAmount)
    {
        NodeDamaged(this, damagedAmount);
    }

    public void InitNode(int nodeId, Vector3 position)
    {
        this.transform.position = position;
        this.NodeId = nodeId;
    }

    public void OnGazeTrigger()
    {
        Debug.Log("Hehe");
        var building = Camera.main.GetComponentInParent<PlayerBuilding>();
        building.FortifyNode(this);

        var movement = Camera.main.GetComponentInParent<PlayerLinearMovement>();
        movement.MoveTo(this);
    }
}
