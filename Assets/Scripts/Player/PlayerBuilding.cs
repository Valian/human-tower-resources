using UnityEngine;
using System.Collections;
using System;

public class PlayerBuilding : MonoBehaviour
{

    public int BuildSpeedPerSecond;
    public bool CanBuild { get { return !movement.IsMoving; } }

    private PlayerLinearMovement movement;
    

    void Start()
    {
        this.movement = this.GetComponent<PlayerLinearMovement>();
    }
    
    public void FortifyNode(Node targetNode)
    {
        if(CanBuild && movement.CurrentNode && targetNode)
        {
            var manager = GameObject.FindObjectOfType<NodeManager>();
            var hpRestored = BuildSpeedPerSecond * Time.deltaTime;
            manager.FortifyEdge(movement.CurrentNode.NodeId, targetNode.NodeId, hpRestored);
        }
    }    
}
